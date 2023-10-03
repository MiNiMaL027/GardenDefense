using Controllers;
using Godot;
using Interfaces;
using System;

public partial class Sickle : RigidBody3D, IPressable
{
    private bool isDragging = false;
    private float linearMovementModifier = 4;

    private float? dragStartY = null;
    private float? dragMouseStartY = null;
    private const float HEIGHT_ERROR_MITIGATION = 0.5f;

    private PlayerController playerController;

    public override void _Ready()
    {
        BodyEntered += Sickle_BodyEntered;
    }

    private void Sickle_BodyEntered(Node body)
    {
        if(body is GrowingPlant plant && plant.Harvestable)
        {
            plant.HarvestToInventory(playerController);
        
            return;
        }
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        SetDeferred("global_rotation", Vector3.Zero);

        CollisionLayer = 2;

        LockRotation = true;
        this.PhysicsMaterialOverride.Friction = 0;
        isDragging = true;

        this.playerController = playerController;
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        isDragging = false;

        this.PhysicsMaterialOverride.Friction = 1;
        CollisionLayer = 1;

        MoveToMouse();

        LockRotation = false;
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        throw new NotImplementedException();
    }

    public override void _PhysicsProcess(double delta)
    {
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
        var query = PhysicsRayQueryParameters3D.Create(from, to, 1);

        var result = spaceState.IntersectRay(query);

        if (result.Count > 0 && (CollisionObject3D)result["collider"] != this)
        {
            Vector3 target = (Vector3)result["position"];

            if (dragStartY == null)
            {
                dragStartY = GlobalPosition.Y; //write object start height
                dragMouseStartY = target.Y; //write mouse start height 
            }

            float mouseCurrentY = target.Y; //write current mouse height
            float differenceBetweenHeights = mouseCurrentY - dragMouseStartY.Value + HEIGHT_ERROR_MITIGATION;

            target.Y = dragStartY.Value + differenceBetweenHeights; //if difference between heights then it affects moving vector
            this.LinearVelocity = linearMovementModifier * (target - GlobalPosition);
        }
    }
}
