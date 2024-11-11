using Controllers;
using Enums;
using Expand;
using Godot;
using Items;
using SaveModels;
using System;
using System.Linq;

public partial class Farm : World
{
    public Area3D FarmArea { get; set; }
    public Funnel Funnel { get; set; }
    public Sickle Sickle { get; set; }
    public MobilePlanforms MobilePlanforms { get; set; }
    public PutArea PutArea { get; set; }
    public override void _Ready()
    {
        base._Ready();
        Funnel = GetNode<Funnel>("Funnel");
        Sickle = GetNode<Sickle>("Sickle");

        MobilePlanforms = GetNode<MobilePlanforms>("Enviroments/Components/MobilePlanforms");
        PutArea = GetNode<PutArea>("PutArea");

        FarmArea = GetNode<Area3D>("FarmArea");
        FarmArea.AreaEntered += FarmArea_AreaEntered;
        FarmArea.AreaExited += FarmArea_AreaExited;
    }
    private void FarmArea_AreaExited(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = false;
        }
    }

    private void FarmArea_AreaEntered(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = true;
        }
    }
    public FarmSave GetFarmSave()
    {
        FarmSave farmSave = new FarmSave()
        {
            SavedItems = new System.Collections.Generic.List<ItemSave>(),
            SavedPots= new System.Collections.Generic.List<PotSave>()
        };
        Godot.Collections.Array<Node> items = GetTree().GetNodesInGroup(Groups.Item);
        foreach (Node n in items)
        {
            if (n.IsInGroup(Groups.Pot))
            {
                Pot p = n as Pot;
                PotSave potSave = new PotSave()
                {
                    ItemId = p.EditorItemId,
                    Transform3D = new TransformSave(p.Transform),
                    WateredLeftTime = p.waterTimer.TimeLeft,
                    FertilizedLeftTime = p.fertilizeTimer.TimeLeft,
                    AppliedFertilizerId = p.Fertilizer?.Id ?? 0,
                    GrowingPlants = p.sockets.Where(s => s.IsUsed == true).Select(s => new GrowingPlantSave(s.GrowingPlant)).ToList()
                };
                farmSave.SavedPots.Add(potSave);
            }
            else
            {
                Item i = n as Item;
                ItemSave itemSave = new ItemSave()
                {
                    ItemId = i.EditorItemId,
                    Transform3D = new TransformSave(i.Transform)
                };
                farmSave.SavedItems.Add(itemSave);
            }
        }
        farmSave.SaveDate = DateTime.Now.ToString(GameSave.ExactDateTimePattern);
        farmSave.MobilePlanformsSave = MobilePlanforms.GetSave();
        return farmSave;
    }

    public void LoadFromSave(FarmSave farmSave)
    {
        ClearWorld();
        DateTime now = DateTime.Now;
        MobilePlanforms.LoadSave(farmSave.MobilePlanformsSave);
        for (int i = 0; i < farmSave.SavedItems.Count; i++)
        {
            ItemDatabaseRow itemDatabaseRow = DbService.GetItem(farmSave.SavedItems[i].ItemId);
            Item item = Item.GetItemSceneByType(itemDatabaseRow.ItemType);
            AddChild(item);
            item.InitializeItem(itemDatabaseRow);
            item.Transform = farmSave.SavedItems[i].Transform3D.GetTransform();
        }
        for (int i = 0; i < farmSave.SavedPots.Count; i++)
        {
            ItemDatabaseRow itemDatabaseRow = DbService.GetItem(farmSave.SavedPots[i].ItemId);
            Pot pot = Item.GetItemSceneByType(itemDatabaseRow.ItemType) as Pot;
            AddChild(pot);
            pot.InitializeItem(itemDatabaseRow);
            pot.LoadFromSave(farmSave.SavedPots[i], DateTime.ParseExact(farmSave.SaveDate, GameSave.ExactDateTimePattern, null), now);
        }
    }
    public override void WorldEnteredListener(PlayerController p)
    {
        GameInstance.Hud.DisplayGardenWidget(p);
        p.CurrentInventory = p.GardenInventory;
        GameInstance.Hud.GardenWidget.UpdateGold(p.Gold);
    }
    public override void WorldExitedListener(PlayerController p)
    {

    }
}
