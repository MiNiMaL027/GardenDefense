using System;
using Controllers;
using Godot;
using Widgets.Inventory;
using Widgets.GardenWidgets;

public static class Scenes
{
    public static class Worlds
    {
        public static World Garden()
        {
            World garden = ResourceLoader.Load<PackedScene>("res://Scenes/Worlds/Garden.tscn", null, ResourceLoader.CacheMode.Ignore).Instantiate<World>();
            return garden;
        }
    }
    public static class Controllers
    {
        public static PlayerController PlayerController()
        {
            PlayerController playerController = ResourceLoader.Load<PackedScene>("res://Scenes/Controllers/PlayerController.tscn").Instantiate<PlayerController>();
            return playerController;
        }
    }
    public static class Widgets
    {
        public static class GardenWidgets
        {
            public static GardenWidget GardenWidget()
            {
                GardenWidget gardenWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/GardenWidgets/GardenWidget.tscn").Instantiate<GardenWidget>();
                return gardenWidget;
            }
        }
        public static class Inventory
        {
            public static InventoryWidget InventoryWidget()
            {
                InventoryWidget inventoryWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventoryWidget.tscn").Instantiate<InventoryWidget>();
                return inventoryWidget;
            }
            public static InventorySlot InventorySlot()
            {
                InventorySlot inventorySlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventorySlot.tscn").Instantiate<InventorySlot>();
                return inventorySlot;
            }
        }
    }
    public static class Items
    {
        public static Item Item()
        {
            Item item = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Item.tscn").Instantiate<Item>();
            return item;
        }
        public static Seed Seed()
        {
            Seed seed = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Seed.tscn").Instantiate<Seed>();
            return seed;
        }
    }
    public static class Pots
    {
        public static Pot Pot()
        {
            Pot pot = ResourceLoader.Load<PackedScene>("res://Scenes/Pots/pot.tscn").Instantiate<Pot>();
            return pot;
        }
    }
    public static Seed Seed()
    {
        Seed seed = ResourceLoader.Load<PackedScene>("res://Scenes/seed.tscn").Instantiate<Seed>();
        return seed;
    }
    public static InventoryComponent InventoryComponent()
    {
        InventoryComponent seed = ResourceLoader.Load<PackedScene>("res://Scenes/InventoryComponent.tscn").Instantiate<InventoryComponent>();
        return seed;
    }
}
