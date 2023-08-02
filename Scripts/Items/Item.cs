using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System;
using System.Collections.Generic;
using Widgets.ContextMenu;

public partial class Item : RigidBody3D, IPressable, IHaveTooltip
{
    #region DragRelatedVariables
    protected bool isDragging = false;
    protected float linearMovementModifier = 4;

    protected float? dragStartY = null;
    protected float? dragMouseStartY = null;
    protected const float HEIGHT_ERROR_MITIGATION = 0.5f;
    #endregion
    
    /// <summary>
    /// This function returns proper scene for requested item type or return null if wrong value
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public static Item GetSceneByType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Misc:
                return Scenes.Items.Item();
            case ItemType.Seed:
                return Scenes.Items.Seed();
            default: return null;
        }
    }
    [Export]
    public int Id
    {
        get { return id; }
        set
        {
            if (value == 0)
                return;

            id = value;
            InitializeItem(id);
            PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
            this.InitVisual(meshScene);
        }
    }
    protected int id;


    BaseTooltip tooltip;

    public int Amount { get; set; }
    public string ItemName { get; set; }
    public string TextureSpritePath { get; set; }
    public string MeshPath { get; set; }

    public string Description { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }
    protected string TooltipScenePath;

    public ItemType ItemType { get; set; }
    public override void _Ready()
    {
        TooltipScenePath = "res://Scenes/Widgets/ToolTip/ItemTooltip.tscn";
        AddToGroup(Groups.Item, true);
        MouseEntered += Item_MouseEntered;
        MouseExited += Item_MouseExited;
    }

    private void Item_MouseExited()
    {
        HideTooltip();

    }

    private void Item_MouseEntered()
    {
        ShowTooltip();
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return Id;
    }
    public static bool operator ==(Item item1, Item item2)
    {
        if (item1 is null)
        {
            return item2 is null;
        }
        if (item2 is null)
        {
            return item1 is null;
        }
        return item1.Id == item2.Id;
    }
    public static bool operator !=(Item item1, Item item2)
    {

        if (item1 is null)
        {
            return !(item2 is null);
        }
        if (item2 is null)
        {
            return !(item1 is null);
        }
        return item1.Id != item2.Id;
    }
    public virtual void InitializeItem(Item itemToCopy)
    {
        id = itemToCopy.Id;
        ItemName = itemToCopy.ItemName;
        BuyPrice = itemToCopy.BuyPrice;
        SellPrice = itemToCopy.SellPrice;

        Description = itemToCopy.Description;
        ItemType = itemToCopy.ItemType;
        TextureSpritePath = itemToCopy.TextureSpritePath;
        MeshPath = itemToCopy.MeshPath;
        this.InitVisual(itemToCopy);
    }
    public virtual void InitializeItem(int itemId)
    {
        ItemDatabaseRow i = DbService.GetItem(itemId);
        InitializeItem(i);
    }
    public virtual void InitializeItem(ItemDatabaseRow i)
    {
        if (i.Id == 0) { return; } //not found
        id = i.Id;
        Amount = 1;
        ItemName = i.ItemName;
        Description = i.Description;
        BuyPrice = i.BuyPrice;
        SellPrice= i.SellPrice;
        ItemType = i.ItemType;
        TextureSpritePath = i.TextureSpritePath;
        MeshPath = i.MeshPath;
        PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
        this.InitVisual(meshScene);
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        SetDeferred("global_rotation", Vector3.Zero);
        LockRotation = true;
        this.PhysicsMaterialOverride.Friction = 0;
        isDragging = true;
        this.CollisionLayer = 0;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (isDragging)
        {
            MoveToMouse();
        }
    }
    protected void MoveToMouse()
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
        TickNotify(result);
    }
    /// <summary>
    /// This function is called during dragging and should provide visible notification to player
    /// </summary>
    public virtual void TickNotify(Godot.Collections.Dictionary raycastResult)
    {

    }
    /// <summary>
    /// This function called when user releases item while dragging
    /// </summary>
    public virtual void TryInteract(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {

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
        TryInteract(eventMouseButton, this.GetPlayerController());
    }
    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        Item_MouseExited();
        ItemContextMenu itemContextMenu = Scenes.Widgets.ContextMenu.ItemContextMenu();
        playerController.Hud.AddAtMousePosition(itemContextMenu);
        itemContextMenu.Init(this, false);
    }
    public void ShowTooltip()
    {
        PackedScene tooltipScene = ResourceLoader.Load<PackedScene>(TooltipScenePath);

        tooltip = tooltipScene.Instantiate<ItemTooltip>();
        PlayerController playerController= this.GetPlayerController();
        playerController.Hud.AddAtMousePosition(tooltip);
        tooltip.ShowTooltip(this);
    }

    public void HideTooltip()
    {
        tooltip.HideTooltip();
        tooltip = null;
    }

    
}
