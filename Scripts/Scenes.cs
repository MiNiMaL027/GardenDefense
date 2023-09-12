using System;
using Controllers;
using Godot;
using Widgets.Inventory;
using Widgets.GardenWidgets;
using Widgets.ContextMenu;
using Widgets.Bestiary;
using Farm.Scripts.Enums;

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
            public static PlantContextMenu PlantContextMenu()
            {
                PlantContextMenu plantContextMenu = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ContextMenu/PlantContextMenu.tscn").Instantiate<PlantContextMenu>();
                return plantContextMenu;
            }
        }

        public static InfoWindow InfoWindow()
        {
            InfoWindow infoWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Global/InfoWindow.tscn").Instantiate<InfoWindow>();
            return infoWindow;
        }

        public static InfoPanel infoPanel()
        {
            InfoPanel infoPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Global/InfoPanel.tscn").Instantiate<InfoPanel>();
            return infoPanel;
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
            public static InventoryAmountWindow InventoryAmountWindow()
            {
                InventoryAmountWindow inventoryAmountWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventoryAmountWindow.tscn").Instantiate<InventoryAmountWindow>();
                return inventoryAmountWindow;
            }
        }

        public static class ToolTip
        {
            public static GrowingPlantTooltip GrowingPlantTooltip()
            {
                GrowingPlantTooltip growingPlantTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/GrowingPlantTooltip.tscn").Instantiate<GrowingPlantTooltip>();
                return growingPlantTooltip;
            }
            public static PotTooltip PotTooltip()
            {
                PotTooltip potTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/PotTooltip.tscn").Instantiate<PotTooltip>();
                return potTooltip;
            }
            public static ItemTooltip ItemTooltip()
            {
                ItemTooltip itemTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/ItemTooltip.tscn").Instantiate<ItemTooltip>();
                return itemTooltip;
            }
        }

        public static class Bestiary
        {
            public static BestiaryWindow BestiaryWindow()
            {
                BestiaryWindow bestiaryWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/BestiaryWindow.tscn").Instantiate<BestiaryWindow>();
                return bestiaryWindow;
            }
        }

        public static class Shop
        {
            public static ShopWindow ShopWindow()
            {
                ShopWindow shopWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/ShopWindow.tscn").Instantiate<ShopWindow>();
                return shopWindow;
            }
            public static shop_slot ShopSlot()
            {
                shop_slot shopSlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/shop_slot.tscn").Instantiate<shop_slot>();
                return shopSlot;
            }
            public static SellWindow SellWindow()
            {
                SellWindow sellWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/SellWindow.tscn").Instantiate<SellWindow>();
                return sellWindow;
            }
            public static sell_slot SellSlot()
            {
                sell_slot sellSlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/sell_slot.tscn").Instantiate<sell_slot>();
                return sellSlot;
            }
            public static movement_slot MovementSlot()
            {
                movement_slot movementSlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/movement_slot.tscn").Instantiate<movement_slot>();
                return movementSlot;    
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
        public static Pot Pot()
        {
            Pot pot = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Pot.tscn").Instantiate<Pot>();
            return pot;
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
