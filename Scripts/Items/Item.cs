using Controllers;
using Enums;
using Farm.Scripts.Enums;
using Godot;
using Interfaces;
using Items;
using Widgets.ContextMenu;

public partial class Item : RigidBody3D, IPressable, IHoverable
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
        }
    }
    protected int id;


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

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return Id;
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
        GD.Print("item id = " + itemId);
        ItemDatabaseRow i = DbService.GetItem(itemId);
        GD.Print("ItemDatabaseRow i = " + i);

        InitializeItem(i);
    }
    public virtual void InitializeItem(ItemDatabaseRow i)
    {
        if (i.Id == 0) { return; } //not found
        id = i.Id;
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
        ItemContextMenu itemContextMenu = Scenes.Widgets.ContextMenu.ItemContextMenu();
        playerController.OpenedContextMenu= itemContextMenu;
        playerController.Hud.AddChild(itemContextMenu);
        itemContextMenu.Init(this, playerController, false);
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

    public void HideTooltip()
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
            tooltip = null;
        }
    }

    public void MouseEnter()
    {
        ShowTooltip();
    }

    public void MouseLeave()
    {
        HideTooltip();
    }
}
