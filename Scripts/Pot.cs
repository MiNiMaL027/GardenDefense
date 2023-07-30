using Controllers;
using Enums;
using Godot;
using Interfaces;
using System;
using System.Collections.Generic;

public partial class Pot : RigidBody3D, IPressable
{
    private OmniLight3D light;
    private bool isSelected;
    private bool isDragging = false;
    private float linearMovementModifier = 1;
    private Node3D soketsContainer;
    public List<PlantSocket> Sokets = new List<PlantSocket>();

    public override void _Ready()
    {
        light = GetNode<OmniLight3D>("Light");
        soketsContainer = GetNode<Node3D>("Sokets");

        this.MouseEntered += RigidBody_MouseEntered;
        this.MouseExited += RigidBody_MouseExited;
       
        AddSockets();
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
        light.Visible = true;
    }

    public void RigidBody_MouseExited()
    {
        isSelected = false;
        
        if(!isDragging)
            light.Visible = false;
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
            this.LinearVelocity= linearMovementModifier*(target - GlobalPosition);
        }
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        GlobalRotation = new Vector3(0, 0, 0);
        isDragging = true;
        LockRotation = true;
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        isDragging = false;
        LockRotation = false;

        MoveToMouse();

        if (!isSelected)
            light.Visible = false;
    }

    private void AddSockets()
    {
        for (int i = 0; i < soketsContainer.GetChildCount(); i++)
        {
            Sokets.Add(soketsContainer.GetChild<PlantSocket>(i));
        }
    }

    public void EnableSockets(SeedType type)
    {
        for (int i = 0; i < soketsContainer.GetChildCount(); i++)
        {
            if (Sokets[i].SeedType == type && !Sokets[i].isUsed)
                Sokets[i].Visible = true;
        }
    }

    public void DisableSockets()
    {
        for (int i = 0; i < soketsContainer.GetChildCount(); i++)
        {
            Sokets[i].Visible = false;
        }
    }
}
