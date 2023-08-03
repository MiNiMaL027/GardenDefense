using Controllers;
using Enums;
using Farm.Scripts.Enums;
using Farm.Scripts.Items;
using Farm.Scripts.Models;
using Godot;
using Interfaces;
using Items;
using System;

public partial class Fertilizer : Item, IPressable
{
    public FertilizerType FertilizerType {get; set;}
    public int NumberOfUses { get; set;}

    public override void TryInteract(InputEventMouseButton eventMouseButton, PlayerController playerController)
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
            var collisionObject = result["collider"].AsGodotObject() as CollisionObject3D;
            if (collisionObject is Pot pot)
            {
                pot.Fertilizer = new FertilizerModel(this);
            }
        }
    }

    public override void InitializeItem(Item i)
    {
        Fertilizer itemToCopy = i as Fertilizer;
        if (itemToCopy == null) { return; }
        id = itemToCopy.Id;
        ItemName = itemToCopy.ItemName;
        BuyPrice = itemToCopy.BuyPrice;
        SellPrice = itemToCopy.SellPrice;
        Description = itemToCopy.Description;
        ItemType = itemToCopy.ItemType;
        MeshPath = itemToCopy.MeshPath;
        TextureSpritePath = itemToCopy.TextureSpritePath;
        FertilizerType = itemToCopy.FertilizerType;
        NumberOfUses = itemToCopy.NumberOfUses;
        this.InitVisual(itemToCopy);
    }
    public override void InitializeItem(int itemId)
    {
        FertilizerDatabaseRow i = DbService.GetItem(itemId) as FertilizerDatabaseRow;
        InitializeItem(i);
    }
    public override void InitializeItem(ItemDatabaseRow dbRow)
    {
        FertilizerDatabaseRow i = dbRow as FertilizerDatabaseRow;
        if (i.Id == 0) { return; } //not found
        id = i.Id;
        Amount = 1;
        ItemName = i.ItemName;
        Description = i.Description;
        BuyPrice = i.BuyPrice;
        SellPrice = i.SellPrice;
        ItemType = i.ItemType;
        MeshPath = i.MeshPath;
        TextureSpritePath = i.TextureSpritePath;
        FertilizerType = i.FertilizerType;
        NumberOfUses = i.NumberOfUses;
        PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
        this.InitVisual(meshScene);
    }
}
