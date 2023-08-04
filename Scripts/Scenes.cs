using System;
using Controllers;
using Godot;
using Widgets.Inventory;
using Widgets.GardenWidgets;
using Widgets.ContextMenu;

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
        public static class ContextMenu
        {
            public static TextureButtonTimeShader TextureButtonTimeShader()
            {
                TextureButtonTimeShader TextureButtonTimeShader = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ContextMenu/TextureButtonTimeShader.tscn").Instantiate<TextureButtonTimeShader>();
                return TextureButtonTimeShader;
            }
            public static ItemContextMenu ItemContextMenu()
            {
                ItemContextMenu itemContextMenu = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ContextMenu/ItemContextMenu.tscn").Instantiate<ItemContextMenu>();
                return itemContextMenu;
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
        public static class ToolTip
        {
            public static GrowingPlantTooltip GrowingPlantTooltip()
            {
                GrowingPlantTooltip growingPlantTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/GrowingPlantTooltip.tscn").Instantiate<GrowingPlantTooltip>();
                return growingPlantTooltip;
            }
        }
        public static WindowConfirmation WindowConfirmation()
        {
            WindowConfirmation windowConfirmation = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/WindowConfirmation.tscn").Instantiate<WindowConfirmation>();
            return windowConfirmation;
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

        public static Fertilizer Fertilizer()
        {
            Fertilizer fertilizer = ResourceLoader.Load<PackedScene>("res://Scenes/Items/fertilizer.tscn").Instantiate<Fertilizer>();
            return fertilizer;
        }
    }

    public static InventoryComponent InventoryComponent()
    {
        InventoryComponent seed = ResourceLoader.Load<PackedScene>("res://Scenes/InventoryComponent.tscn").Instantiate<InventoryComponent>();
        return seed;
    }
    public static GrowingPlant GrowingPlant()
    {
        GrowingPlant growingPlant = ResourceLoader.Load<PackedScene>("res://Scenes/GrowingPlant.tscn").Instantiate<GrowingPlant>();
        return growingPlant;
    }
}
