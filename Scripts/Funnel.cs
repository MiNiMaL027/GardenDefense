using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Funnel : RigidBody3D, IPressable
{
    private bool isSelected;
    private bool isDragging = false;
    private float linearMovementModifier = 4;

    private float? dragStartY = null;
    private float? dragMouseStartY = null;
    private const float HEIGHT_ERROR_MITIGATION = 0.5f;

    public override void _Ready()
    {
    }
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        this.GlobalRotation = new Vector3(0, 0, 0);
        LockRotation = true;
        this.PhysicsMaterialOverride.Friction = 0;
        isDragging = true;
        this.CollisionLayer = 0;
    }
    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        isDragging = false;

        this.PhysicsMaterialOverride.Friction = 1;

        MoveToMouse();
        dragStartY = null;
        dragMouseStartY = null;
        LockRotation = false;
        this.CollisionLayer = 1;

    }
    public override void _PhysicsProcess(double delta)
    {
        //GD.Print(LinearVelocity);
        if (isDragging)
        {
            MoveToMouse();
        }
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
            if (dragStartY == null)
            {
                dragStartY = GlobalPosition.Y; //write object start height
                dragMouseStartY = target.Y; //write mouse start height 
            }
            GD.Print("LPt:" + target);
            GD.Print("GP:" + GlobalPosition);
            GD.Print("LV:" + this.LinearVelocity);

            float mouseCurrentY = target.Y; //write current mouse height
            float differenceBetweenHeights = mouseCurrentY - dragMouseStartY.Value + HEIGHT_ERROR_MITIGATION;
            target.Y = dragStartY.Value + differenceBetweenHeights; //if difference between heights then it affects moving vector
            this.LinearVelocity = linearMovementModifier * (target - GlobalPosition);
        }
    }
}
