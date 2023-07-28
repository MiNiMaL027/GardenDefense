using Godot;
using System;

public partial class FunnelV2 : RigidBody3D
{
    private bool isSelected;
    private bool isDragging = false;
    private float linearMovementModifier = 2;

    private float? dragStartY = null;
    private float? dragMouseStartY = null;
    private float? meshMouseYDelta = null;



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
                LeftMouseDownListener(eventMouseButton);
            }
            else
            {
                LeftMouseUpListener(eventMouseButton);
            }
        }
    }
    private void LeftMouseDownListener(InputEventMouseButton eventMouseButton)
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
                this.GlobalRotation = new Vector3(0, 0, 0);
                LockRotation = true;
                this.PhysicsMaterialOverride.Friction = 0;
                isDragging = true;
            }
        }
    }
    private void LeftMouseUpListener(InputEventMouseButton eventMouseButton)
    {
        GD.Print("LeftMouseUpListener");
        isDragging = false;
        LockRotation = false;
        dragStartY = null;
        dragMouseStartY = null;
        meshMouseYDelta = null;
        this.PhysicsMaterialOverride.Friction = 1;
        MoveToMouse();

    }
    public override void _PhysicsProcess(double delta)
    {
        if (isDragging)
        {
            MoveToMouse();
        }
    }

    public void RigidBody_MouseEntered()
    {
        isSelected = true;
    }

    public void RigidBody_MouseExited()
    {
        isSelected = false;

    }

    private void MoveToMouse()
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
            if(dragStartY == null)
            {
                dragStartY = GlobalPosition.Y; //write object start height
                dragMouseStartY = target.Y; //write mouse start height 
                meshMouseYDelta = dragMouseStartY- dragStartY; //write required difference between heights
            }
            float mouseCurrentY = target.Y; //write current mouse height
            float differenceBetweenHeights = mouseCurrentY - dragMouseStartY.Value;
            target.Y += differenceBetweenHeights; //if difference between heights then it affects moving vector
            this.LinearVelocity = linearMovementModifier * (target - GlobalPosition);
            dragMouseStartY += GlobalPosition.Y - dragStartY;
            dragStartY = GlobalPosition.Y;
        }
    }
}
