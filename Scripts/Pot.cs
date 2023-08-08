using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System.Collections.Generic;

public partial class Pot : RigidBody3D, IPressable, IHoverable
{
    private OmniLight3D light;
    private bool isSelected;
    private bool isDragging = false;
    private float linearMovementModifier = 1;
    private Node3D socketsContainer;
    public Node3D plantsContainer;
    public Timer waterTimer;
    public Timer fertilizeTimer;
    private int secondsTimeToDry = 300;
    public List<PlantSocket> sockets;
    public MeshInstance3D mesh;
    public PotTooltip tooltip;

    private FertilizerDatabaseRow fertilizer;
    public FertilizerDatabaseRow Fertilizer
    {
        get { return fertilizer; }

        set 
        {
            //can't add another fertilizer and can't assign no furtilizer
            if (value == null || fertilizer != null) { return; }
            fertilizer = value;
            fertilizeTimer.WaitTime = fertilizer.SecondsDuration;
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

            ChangeVisualWateredOrNot(value);

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
        mesh = GetNode<Node3D>("Mesh").GetChild<MeshInstance3D>(0);

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
        AddChild(fertilizeTimer);
        fertilizeTimer.Timeout += FertilizeTimer_Timeout;
        #endregion
       
        ReadSockets();
    }

    private void FertilizeTimer_Timeout()
    {
        fertilizer = null;
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

    public void MouseEnter()
    {      
        isSelected = true;
        light.Visible = true;
        ShowTooltip();
        
    }

    public void MouseLeave()
    {
        isSelected = false;
        
        if(!isDragging)
            light.Visible = false;
        HideTooltip();

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
            if (sockets[i].SeedType == type && !sockets[i].IsUsed)
            {
                sockets[i].Visible = true;
                sockets[i].CollisionLayer= 1;
                sockets[i].CollisionMask = 1;

            }
        }
    }

    public void DisableSockets()
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].Visible = false;
            sockets[i].CollisionLayer = 0;
            sockets[i].CollisionMask = 0;
        }
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
    }

    private void ChangeVisualWateredOrNot(bool watered)
    {
        if(watered)
        {
            mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/WaterDirt_Material.tres"));
        }
        else
        {
            mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/Dirt_Material.tres"));
        }
    }

    public void ShowTooltip()
    {
        tooltip = Scenes.Widgets.ToolTip.PotTooltip();
        PlayerController playerController = this.GetPlayerController();
        playerController.Hud.AddChild(tooltip);
        tooltip.ShowTooltip(this);
        playerController.Hud.AddAtMousePosition(tooltip);

    }

    public void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.QueueFree();
            tooltip = null;
        }
    }
}