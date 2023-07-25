using Enums;
using Godot;
using Items;
using System;

public partial class Seed : Item
{
    public SeedType SeedType { get; set; }
    public int StagesAmount { get; set; }
    public int MinSecondsToChangeState { get; set; }
    public int MaxSecondsToChangeState { get; set; }
    public int GrowUpId { get; set; }
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
        Init(itemToCopy);
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
        Init(meshScene);
    }
}
