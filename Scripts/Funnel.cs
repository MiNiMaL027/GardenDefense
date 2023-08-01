using Controllers;
using Enums;
using Godot;
using Interfaces;
using System;

public partial class Funnel : RigidBody3D, IPressable
{
    private bool isDragging = false;
    private float linearMovementModifier = 4;
    private bool isInteractable = true;

    private float? dragStartY = null;
    private float? dragMouseStartY = null;
    private const float HEIGHT_ERROR_MITIGATION = 0.5f;

    public int currentNumberOfWater = 1;
    public int maxNumberOfWater = 2;

    private AnimationPlayer animation;

    public override void _Ready()
    {
        animation = GetNode<AnimationPlayer>("Animation");
        animation.AnimationFinished += Animation_AnimationFinished;
    }
    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if (!isInteractable)
            return;

        SetDeferred("global_rotation", Vector3.Zero);

        LockRotation = true;
        this.PhysicsMaterialOverride.Friction = 0;
        isDragging = true;
        this.CollisionLayer = 0;
        Freeze = false;
    }
    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if (!isInteractable)
            return;

        isDragging = false;

        this.PhysicsMaterialOverride.Friction = 1;

        MoveToMouse();

        dragStartY = null;
        dragMouseStartY = null;
        LockRotation = false;
        this.CollisionLayer = 1;

        TryInteract(eventMouseButton, playerController);
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

            float mouseCurrentY = target.Y; //write current mouse height
            float differenceBetweenHeights = mouseCurrentY - dragMouseStartY.Value + HEIGHT_ERROR_MITIGATION;
            target.Y = dragStartY.Value + differenceBetweenHeights; //if difference between heights then it affects moving vector
            this.LinearVelocity = linearMovementModifier * (target - GlobalPosition);
        }
    }

    /// <summary>
    /// This function called when user releases item while dragging
    /// </summary>
    public void TryInteract(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        Vector2 mousePosition = eventMouseButton.GlobalPosition;
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0 )
        {
            var collisionObject = result["collider"].AsGodotObject() as RigidBody3D;
            GD.Print(collisionObject);
            if(collisionObject is Pot pot && currentNumberOfWater > 0)
            {
                animation.Play("Water");
                pot.Watered = true;
                GD.Print(pot.Watered);
                this.LinearVelocity = Vector3.Zero;             
                Freeze = true;
                isInteractable = false;
            }     
        }
    }

    public void FillWithWater()
    {
        currentNumberOfWater = maxNumberOfWater;
    }

    #region animation
    private void Animation_AnimationFinished(StringName animName)
    {
        isInteractable = true;
        Freeze = false;
    }
    #endregion
}
