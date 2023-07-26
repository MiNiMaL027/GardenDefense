using Godot;
using System;

public partial class Funnel : RigidBody3D
{
    private bool isDragging = false;
    private bool isSelected;
    private float linearMovementModifier = 2;
    public int currentWater {get;set;}

    public override void _Ready()
	{
        MouseEntered += RigidBody_MouseEntered;
        MouseExited += RigidBody_MouseExited;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == MouseButton.Left)
        {
            if (eventMouseButton.Pressed)
            {
                Vector2 mousePosition = eventMouseButton.Position;

                PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
                Camera3D camera = GetViewport().GetCamera3D();
                Vector3 from = camera.ProjectRayOrigin(mousePosition);
                Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
                var query = PhysicsRayQueryParameters3D.Create(from, to);
                var result = spaceState.IntersectRay(query);


                if (result.Count > 0)
                {
                    RigidBody3D resultBody = result["collider"].AsGodotObject() as RigidBody3D;
                    if (resultBody == this)
                    {
                        this.CustomIntegrator = true;
                        this.GlobalRotation = new Vector3(0, 0, 0);                      
                        LockRotation = true;
                        //light.Visible = true;
                        isDragging = true;
                    }
                }
            }
            else
            {
                this.CustomIntegrator = false;
                isDragging = false;
                LockRotation = false;

                //if (!isSelected)
                    //light.Visible = false;
            }
        }
    }


    public override void _Process(double delta)
	{
        if (isDragging)
        {
            MoveToMouse(delta);
        }
    }

    public void RigidBody_MouseEntered()
    {
        isSelected = true;
        //light.Visible = true;
    }

    public void RigidBody_MouseExited()
    {
        isSelected = false;

        //if (!isDragging)
        //    light.Visible = false;
    }

    private void MoveToMouse(double delta)
    {
        Vector2 mousePosition = GetViewport().GetMousePosition();

        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;

        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0 && (CollisionObject3D)result["collider"] != this)
        {
            Vector3 target = (Vector3)result["position"];
            this.LinearVelocity = linearMovementModifier * (target - GlobalPosition);


            Transform = new Transform3D(Basis.Identity, new Vector3(
                Transform.Origin.X,
                Mathf.Lerp(Transform.Origin.Y, 5, linearMovementModifier * (float)delta),
                Transform.Origin.Z
            ));
        }
    }
}
