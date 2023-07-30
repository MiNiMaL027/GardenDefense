using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System;

[Tool]
public partial class Item : RigidBody3D, IPressable
{
    #region DragRelatedVariables
    protected bool isDragging = false;
    protected float linearMovementModifier = 4;

    protected float? dragStartY = null;
    protected float? dragMouseStartY = null;
    protected const float HEIGHT_ERROR_MITIGATION = 0.5f;
    #endregion
    public void Init(PackedScene meshSceneToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = this.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            Node n = children[i] as Node;
            n.QueueFree();
        }
        if (meshSceneToLoad == null) { return; }

        ///add mesh to scene
        Node3D meshToLoad = meshSceneToLoad.Instantiate<Node3D>();
        AddChild(meshToLoad);


        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, this);
        meshToLoad.QueueFree();
    }
    public void Init(Node3D meshToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = this.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            Node n = children[i];
            n.QueueFree();
        }
        if (meshToLoad == null) { return; }
        AddChild(meshToLoad);

        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, this);
        meshToLoad.QueueFree();
    }
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
    public int Id
    {
        get { return id; }
        set
        {
            id = value;
            InitializeItem(id);
            PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
            Init(meshScene);
        }
    }
    protected int id;
    public int Amount { get; set; }
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
        Init(itemToCopy);
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
        Init(meshScene);
    }
    private void MigrateCollisionsAndMeshes(Node target, Vector3 scale, Node newParent)
    {
        Godot.Collections.Array<Node> children = target.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is CollisionShape3D collisionShape)
            {
                collisionShape.RemoveFromParent();
                newParent.AddChild(collisionShape);
                collisionShape.Scale *= scale;
            }
            else if (children[i] is MeshInstance3D meshInstance)
            {
                meshInstance.RemoveFromParent();
                newParent.AddChild(meshInstance);
                meshInstance.Scale *= scale;
            }
            else
            {
                if (children[i] is Node3D spatial)
                {
                    MigrateCollisionsAndMeshes(children[i], scale * spatial.Scale, newParent);

                }
                else
                {
                    MigrateCollisionsAndMeshes(children[i], scale, newParent);
                }
            }
        }
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        this.GlobalRotation = new Vector3(0, 0, 0);
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
}
