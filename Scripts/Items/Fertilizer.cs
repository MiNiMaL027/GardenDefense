using Controllers;
using Enums;
using Godot;
using Interfaces;
using Items;
using System;

public partial class Fertilizer : Item, IPressable
{
    public FertilizerType FertilizerType {get; set;}
    public int SecondsDuration { get; set;}

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
            StaticBody3D body = result["collider"].AsGodotObject() as StaticBody3D;
            var collisionObject = result["collider"].AsGodotObject() as CollisionObject3D;

            if (collisionObject is Pot pot && pot.Fertilizer == null && pot.plantsContainer.GetChildCount() == 0)
            {               
                pot.Fertilizer = DbService.GetItem(EditorItemId) as FertilizerDatabaseRow;

                QueueFree();
            }           
            else if (body is Ambar)
            {
                this.MoveToInventory(playerController);
            }
        }
    }

    public override void InitializeItem(Item i)
    {
        Fertilizer itemToCopy = i as Fertilizer;

        if (itemToCopy == null) { return; }

        editorItemId = itemToCopy.EditorItemId;
        ItemName = itemToCopy.ItemName;
        BuyPrice = itemToCopy.BuyPrice;
        SellPrice = itemToCopy.SellPrice;
        Description = itemToCopy.Description;
        ItemType = itemToCopy.ItemType;
        MeshPath = itemToCopy.MeshPath;
        TextureSpritePath = itemToCopy.TextureSpritePath;
        FertilizerType = itemToCopy.FertilizerType;
        SecondsDuration = itemToCopy.SecondsDuration;

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

        editorItemId = i.Id;
        ItemName = i.ItemName;
        Description = i.Description;
        BuyPrice = i.BuyPrice;
        SellPrice = i.SellPrice;
        ItemType = i.ItemType;
        MeshPath = i.MeshPath;
        TextureSpritePath = i.TextureSpritePath;
        FertilizerType = i.FertilizerType;
        SecondsDuration = i.SecondsDuration;
        PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);

        this.InitVisual(meshScene);
    }
}
