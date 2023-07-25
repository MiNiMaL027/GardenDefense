using Controllers;
using Enums;
using Godot;
using Items;
using System;

[Tool]
public partial class Item : RigidBody3D
{
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
}
