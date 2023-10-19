using Controllers;
using Enums;
using Farm.Scripts.Enums;
using Farm.Scripts.Widgets.ToolTip;
using Godot;
using Interfaces;
using Items;
using System.Collections.Generic;
using Widgets.ContextMenu;

public partial class Item : RigidBody3D, IPressable, IHoverable
{
    #region DragRelatedVariables
    public bool isDragging = false;
    protected float linearMovementModifier = 4;

    protected float? dragStartY = null;
    protected float? dragMouseStartY = null;
    protected const float HEIGHT_ERROR_MITIGATION = 0.5f;

    protected uint MainLayer = 1;
    protected uint MoveLayer = 0;
    #endregion

    /// <summary>
    /// This function returns proper scene for requested item type or return null if wrong value
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public static Item GetItemSceneByType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Misc:
                return Scenes.Items.Item();
            case ItemType.Seed:
                return Scenes.Items.Seed();
            case ItemType.Fertilizer:
                return Scenes.Items.Fertilizer();
            case ItemType.Harvestable:
                return Scenes.Items.Item();
            case ItemType.Pot:
                return Scenes.Items.Pot();
            default: return null;
        }
    }
    public static ItemTooltip GetTooltipSceneByType(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Misc:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            case ItemType.Seed:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            case ItemType.Fertilizer:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            case ItemType.Harvestable:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            case ItemType.Pot:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            case ItemType.BattlePlant:
                return Scenes.Widgets.ToolTip.ItemTooltip();
            default: return null;
        }
    }
    /// <summary>
    /// It should be initialized in EnterTree event. Only assign id value here
    /// </summary>
    [Export]
    public int EditorItemId
    {
        get { return editorItemId; }
        set
        {
            editorItemId = value;
        }
    }
    protected int editorItemId;
    protected bool isInitedFromEditor = false;


    ItemTooltip tooltip;

    public string ItemName { get; set; }
    public string TextureSpritePath { get; set; }
    public string MeshPath { get; set; }

    public string Description { get; set; }
    public int BuyPrice { get; set; }
    public int SellPrice { get; set; }

    public ItemType ItemType { get; set; }
    public override void _Ready()
    {
        AddToGroup(Groups.Item, true);
    }

    public void PickTimer_Timeout()
    {
        if (isDragging)
            return;

        MoveToInventory(this.GetPlayerController());
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return EditorItemId;
    }
    public static bool operator == (Item item1, Item item2)
    {
        if (item1 is null)
        {
            return item2 is null;
        }
        if (item2 is null)
        {
            return item1 is null;
        }

        return item1.EditorItemId == item2.EditorItemId;
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

        return item1.EditorItemId != item2.EditorItemId;
    }
    public virtual void InitializeItem(Item itemToCopy)
    {
        editorItemId = itemToCopy.EditorItemId;
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

        editorItemId = i.Id;
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

    public virtual void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        SetDeferred("global_rotation", Vector3.Zero);

        LockRotation = true;
        this.PhysicsMaterialOverride.Friction = 0;
        isDragging = true;
        this.CollisionLayer = MoveLayer;
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
        Vector2 mousePosition = eventMouseButton.GlobalPosition;
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = false;
        query.CollideWithBodies = true;
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0)
        {
            StaticBody3D body = result["collider"].AsGodotObject() as StaticBody3D;

            if(body is Ambar)
            {
                this.MoveToInventory(playerController);
            }           
        }
    }
    public virtual void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        isDragging = false;

        this.PhysicsMaterialOverride.Friction = 1;

        MoveToMouse();

        dragStartY = null;
        dragMouseStartY = null;
        LockRotation = false;
        this.CollisionLayer = MainLayer;

        TryInteract(eventMouseButton, this.GetPlayerController());
    }
    public virtual void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        ItemContextMenu itemContextMenu = Scenes.Widgets.ContextMenu.ItemContextMenu();

        playerController.OpenedContextMenu = itemContextMenu;

        playerController.Hud.AddChild(itemContextMenu);
        itemContextMenu.Init(this, null, playerController, false);
        playerController.Hud.AddAtMousePosition(itemContextMenu);
    }
    public void ShowTooltip()
    {
        tooltip = GetTooltipSceneByType(ItemType);

        PlayerController playerController = this.GetPlayerController();

        playerController.Hud.AddChild(tooltip);      
        tooltip.ShowTooltip(this);
        playerController.Hud.AddAtMousePosition(tooltip);
    }

    public virtual void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();

            tooltip = null;
        }
    }

    public virtual void MouseEnter()
    {
        ShowTooltip();
    }

    public void MouseLeave()
    {
        HideTooltip();
    }
    public override void _EnterTree()
    {
        base._EnterTree();

        if (EditorItemId == 0 || isInitedFromEditor==true)
            return;

        InitializeItem(editorItemId);
        isInitedFromEditor= true;
    }

    public virtual void MoveToInventory(PlayerController controller)
    {
        controller.InventoryComponentSeeds.AddItem(this.EditorItemId, 1);

        controller.Hud.GardenWidget.InfoWindow.AddInfoPanel($"{this.ItemName} - Added to inventory", this.TextureSpritePath);

        this.QueueFree();
    }
}
