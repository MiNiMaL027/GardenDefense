using Controllers;
using Enums;
using Interfaces;
using Godot;
using System;
using Items;
using BaseClasses;

public partial class Funnel : BaseRigidBody3D, IPressable, IUpgradable
{
    MeshInstance3D meshInstance;

    private bool isDragging = false;
    private float linearMovementModifier = 4;
    public bool isInteractable = true;

    private float? dragStartY = null;
    private float? dragMouseStartY = null;
    private const float HEIGHT_ERROR_MITIGATION = 0.5f;

    private int _currentNumberOfWater = 1;
    public int currentNumberOfWater
    {
        get
        {
            return _currentNumberOfWater;
        }
        set
        {
            _currentNumberOfWater = value;

            if (_currentNumberOfWater == 0)
            {
                waterCountLabel.Text = "Empty";
                waterCountLabel.Position += new Vector3(0.25f, 0, 0);
            }
            else
            {
                waterCountLabel.Text = _currentNumberOfWater.ToString();
                waterCountLabel.Position = new Vector3(0.098f, 0.553f, 0);
            }
        }
    }
    public int maxNumberOfWater = 2;

    private AnimationPlayer animation;
    private GpuParticles3D particle;

    private Timer dropTimer;

    private Label3D waterCountLabel;
    private Node3D waterWidget;

    public int CountOfAvalibalUpgrades { get; set; } = 2;
    public int CostToUpgrade { get; set; } = 10;

    public override void _Ready()
    {
        meshInstance = GetNode<MeshInstance3D>("watering_can/Куб");
        animation = GetNode<AnimationPlayer>("Animation");
        particle = GetNode<GpuParticles3D>("Particle");

        waterCountLabel = GetNode<Label3D>("3dControl/Label3D");
        waterWidget = GetNode<Node3D>("3dControl");

        waterCountLabel.Text = _currentNumberOfWater.ToString();

        #region dropTimer

        dropTimer = new Timer();
        dropTimer.Autostart = false;
        dropTimer.OneShot = true;
        dropTimer.WaitTime = 2;
        AddChild(dropTimer);
        dropTimer.Timeout += DropTimer_Timeout;

        #endregion

        animation.AnimationFinished += Animation_AnimationFinished;
        BodyEntered += Funnel_BodyEntered;

        base._Ready();
    }

    private void DropTimer_Timeout()
    {
        Freeze = false;
    }

    private void Funnel_BodyEntered(Node body)
    {
        if (body is Item)
            return;

        dragStartY = null;
        dragMouseStartY = null;
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

        waterWidget.Visible = true;

        dropTimer.Stop();
    }
    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        if (!isInteractable)
            return;

        isDragging = false;

        this.PhysicsMaterialOverride.Friction = 1;

        MoveToMouse();

        //dragStartY = null;
        //dragMouseStartY = null;
        LockRotation = false;
        this.CollisionLayer = 1;

        waterWidget.Visible = false;

        TryInteract(eventMouseButton, playerController);
    }
    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
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
            var collisionObject = result["collider"].AsGodotObject() as CollisionObject3D;

            if(collisionObject is Pot pot && currentNumberOfWater > 0)
            {
                CollisionLayer = 0;
                animation.Play("Water");

                pot.Watered = true;
                this.LinearVelocity = Vector3.Zero;
                currentNumberOfWater--;
                Freeze = true;
                isInteractable = false;
            } 
            else if(collisionObject is Pot && (currentNumberOfWater <= 0))
            {
                playerController.Hud.GardenWidget.InfoWindow.AddInfoPanel("The funnel is empty, please fill it with water to be able to water");
            }
            else if(collisionObject is WaterPump pump)
            {
                pump.FillFunnel(this);
            }
        }
    }

    public void FillWithWater()
    {
        currentNumberOfWater = maxNumberOfWater;
    }

    public void Upgrade()
    {
        var playerController = this.GetPlayerController();
        if(CountOfAvalibalUpgrades > 0 && playerController.Gold >= CostToUpgrade)
        {
            playerController.Gold -= CostToUpgrade;
            CountOfAvalibalUpgrades--;

            CostToUpgrade *= 2;
            maxNumberOfWater += 2;
            var material = (meshInstance.Mesh.SurfaceGetMaterial(0) as StandardMaterial3D);
            material.AlbedoColor = new Color(material.AlbedoColor.R, material.AlbedoColor.G - 0.1f, material.AlbedoColor.B, material.AlbedoColor.A);
        }
    }

    #region animation

    private void Animation_AnimationFinished(StringName animName)
    {
        isInteractable = true;

        dropTimer.Start(0);

        CollisionLayer = 1;
    }
    
    #endregion
}
