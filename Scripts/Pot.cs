using Controllers;
using Enums;
using Farm.Scripts.Models;
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
    private Node3D socketsContainer;
    public Node3D plantsContainer;
    private Timer waterTimer;
    private Timer fertilizeTimer;
    private int secondsTimeToDry = 300;
    public List<PlantSocket> sockets;

    private FertilizerModel? fertilizer;
    public FertilizerModel? Fertilizer
    {
        get { return fertilizer; }

        set 
        {
            fertilizer = value;

            fertilizeTimer.Start();
        }
    }
    private bool watered;
    public bool Watered
    {
        get { return watered; }
        set
        {
            waterTimer.Start(); 
            watered = value;
            //TODO change visual to watered or not watered

            Godot.Collections.Array<Node> plantsGdArray = plantsContainer.GetChildren();

            for (int i = 0; i < plantsGdArray.Count; i++)
            {
                (plantsGdArray[i] as GrowingPlant).SetWatered(value);
            }
        }
    }

    public override void _Ready()
    {
        light = GetNode<OmniLight3D>("Light");
        socketsContainer = GetNode<Node3D>("Soсkets");
        plantsContainer = GetNode<Node3D>("Plants");

        #region waterTimer
        waterTimer = new Timer();
        waterTimer.Autostart = false;
        waterTimer.WaitTime = secondsTimeToDry;
        AddChild(waterTimer);
        waterTimer.Timeout += WaterTimer_Timeout;
        #endregion

        #region fertilizeTimer
        fertilizeTimer = new Timer();
        fertilizeTimer.Autostart = false;
        fertilizeTimer.WaitTime = secondsTimeToDry;
        AddChild(fertilizeTimer);
        fertilizeTimer.Timeout += FertilizeTimer_Timeout;
        #endregion

        this.MouseEntered += RigidBody_MouseEntered;
        this.MouseExited += RigidBody_MouseExited;
       
        ReadSockets();
    }

    private void FertilizeTimer_Timeout()
    {
        if(fertilizer.NumberOfUses > 0)
        {
            fertilizer.NumberOfUses--;
            fertilizeTimer.Start();
        }
        else
        {
            fertilizer = null;
        }
    }

    private void WaterTimer_Timeout()
    {
        Watered = false;
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
            this.LinearVelocity= linearMovementModifier* new Vector3(target.X - GlobalPosition.X,0,target.Z - GlobalPosition.Z);
        }        
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        SetDeferred("global_rotation", Vector3.Zero);
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

    private void ReadSockets()
    {
        Godot.Collections.Array<Node> socketsGdArray = socketsContainer.GetChildren();
        sockets = new List<PlantSocket>(socketsGdArray.Count);
        for (int i = 0; i < socketsGdArray.Count; i++)
        {
            sockets.Add(socketsGdArray[i] as PlantSocket);
        }
    }

    public void EnableSockets(SeedType type)
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            if (sockets[i].SeedType == type && !sockets[i].isUsed)
                sockets[i].Visible = true;
        }
    }

    public void DisableSockets()
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].Visible = false;
        }
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        throw new NotImplementedException();
    }
}