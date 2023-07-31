using Controllers;
using Enums;
using Godot;
using Godot.Collections;
using Interfaces;
using Items;
using System;

public partial class Seed : Item
{
    public SeedType SeedType { get; set; }
    public int StagesAmount { get; set; }
    public int MinSecondsToChangeState { get; set; }
    public int MaxSecondsToChangeState { get; set; }
    public int GrowUpId { get; set; }
    public Pot DragCurrentPot { get; set; }
    public override void _Ready()
    {
        base._Ready();
    }
    public override void InitializeItem(Item i)
    {
        Seed itemToCopy = i as Seed;
        if (itemToCopy == null) { return; }
        id = itemToCopy.Id;
        ItemName = itemToCopy.ItemName;
        BuyPrice = itemToCopy.BuyPrice;
        SellPrice = itemToCopy.SellPrice;
        Description = itemToCopy.Description;
        ItemType = itemToCopy.ItemType;
        MeshPath = itemToCopy.MeshPath;
        TextureSpritePath = itemToCopy.TextureSpritePath;
        SeedType= itemToCopy.SeedType;
        StagesAmount = itemToCopy.StagesAmount;
        MinSecondsToChangeState = itemToCopy.MinSecondsToChangeState;
        MaxSecondsToChangeState = itemToCopy.MaxSecondsToChangeState;
        GrowUpId = itemToCopy.GrowUpId;
        this.InitVisual(itemToCopy);
    }
    public override void InitializeItem(int itemId)
    {
        SeedDatabaseRow i = DbService.GetItem(itemId) as SeedDatabaseRow;
        InitializeItem(i);
    }
    public override void InitializeItem(ItemDatabaseRow dbRow)
    {
        SeedDatabaseRow i = dbRow as SeedDatabaseRow;
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
        SeedType = i.SeedType;
        StagesAmount = i.StagesAmount;
        MinSecondsToChangeState = i.MinSecondsToChangeState;
        MaxSecondsToChangeState = i.MaxSecondsToChangeState;
        GrowUpId = i.GrowUpId;
        PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
        this.InitVisual(meshScene);
    }

    public override void TickNotify(Dictionary raycastResult)
    {
        if (raycastResult.Count > 0)
        {
            if ((CollisionObject3D)raycastResult["collider"] is Pot targetPot)
            {
                if (targetPot == DragCurrentPot)
                    return;

                DragCurrentPot?.DisableSockets();
                DragCurrentPot = targetPot;
                DragCurrentPot.EnableSockets(SeedType);
            }
            else
            {
                DragCurrentPot?.DisableSockets();
                DragCurrentPot = null;
            }
        }
        else
        {
            DragCurrentPot?.DisableSockets();
            DragCurrentPot = null;
        }
    }

    public override void TryInteract(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {

        Vector2 mousePosition = eventMouseButton.GlobalPosition;
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = true;
        query.CollideWithBodies = false;
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            Area3D area = result["collider"].AsGodotObject() as Area3D;
            if (area is PlantSocket plantSocket && plantSocket.SeedType == SeedType && plantSocket.isUsed == false)
            {
                plantSocket.Plant(this);
            }
        }
    }
}
