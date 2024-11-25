using AI;
using Components;
using Controllers;
using Godot;
using Items;
using Particles;
using Projectiles;
using Widgets.BattleAgent;
using Widgets.Bestiary;
using Widgets.ContextMenu;
using Widgets.GardenWidgets;
using Widgets.Global;
using Widgets.Inventory;
using Widgets.Shop;
using Widgets.Shop.Expand;
using Widgets.Shop.Upgrade;
using Widgets.ToolTip;
using static Notifications;

public static class Scenes
{
    public static class Components
    {
        public static ProgressBar3D ProgressBar3D()
        {
            ProgressBar3D progressBar = ResourceLoader.Load<PackedScene>("res://Scenes/Components/ProgressBar3D.tscn", null).Instantiate<ProgressBar3D>();
            return progressBar;
        }
    }
    public static class Worlds
    {
        public static Farm Farm()
        {
            Farm garden = ResourceLoader.Load<PackedScene>("res://Scenes/Worlds/Garden.tscn", null, ResourceLoader.CacheMode.Ignore).Instantiate<Farm>();
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
        public static class Monsters
        {
            public static AntAIController TestMonsterAIController()
            {
                AntAIController TestMonsterAIController = ResourceLoader.Load<PackedScene>("res://Scenes/Controllers/Monsters/AntAIController.tscn").Instantiate<AntAIController>();
                return TestMonsterAIController;
            }
        }
    }

    public static class Projectiles
    {
        public static class BattlePea
        {
            public static Projectile PeaMainProjectile()
            {
                Projectile projectile = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/BattlePlants/Pea/PeaMainProjectile.tscn").Instantiate<Projectile>();
                return projectile;
            }
            public static Projectile PeaAdditionalProjectile()
            {
                Projectile projectile = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/BattlePlants/Pea/PeaAdditionalProjectile.tscn").Instantiate<Projectile>();
                return projectile;
            }
        }
        public static class BattleCorn
        {
            public static SelfAimingProjectile CornMainProjectile()
            {
                SelfAimingProjectile projectile = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/BattlePlants/Corn/CornMainProjectile.tscn").Instantiate<SelfAimingProjectile>();
                return projectile;
            }
        }
        public static class WhiteLily
        {
            public static BallisticProjectile WhiteLilyMainProjectile()
            {
                BallisticProjectile projectile = ResourceLoader.Load<PackedScene>("res://Scenes/Projectiles/BattlePlants/Lily/LilyMainProjectile.tscn").Instantiate<BallisticProjectile>();
                return projectile;
            }
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
            public static BattlefieldWidget BattlefieldWidget()
            {
                BattlefieldWidget bw = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/GardenWidgets/BattlefieldWidget.tscn").Instantiate<BattlefieldWidget>();
                return bw;
            }
            public static HpBar HpBar()
            {
                HpBar hpBar = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/GardenWidgets/HpBar.tscn").Instantiate<HpBar>();
                return hpBar;
            }
        }

        public static class Skills
        {
            public static SkillWindow SkillWindow()
            {
                var skill = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Skills/SkillWindow.tscn").Instantiate<SkillWindow>();
                return skill;
            }

            public static ChooseSkillWidget ChooseSkillWidget()
            {
                var skillWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Skills/ChooseSkillWidget.tscn").Instantiate<ChooseSkillWidget>();
                return skillWidget;
            }
        }

        public static class Laboratory
        {
            public static LaboratoryWindow LaboratoryWindow()
            {
                LaboratoryWindow laboratiryWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Laboratory/LaboratoryWindow.tscn").Instantiate<LaboratoryWindow>();
                return laboratiryWindow;
            }
        }

        public static UnlockWindow UnlockWindow()
        {
            UnlockWindow unlockWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Global/UnlockWindow.tscn").Instantiate<UnlockWindow>();
            return unlockWindow;
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
            public static BattlePlantContextMenu BattlePlantContextMenu()
            {
                BattlePlantContextMenu plantContextMenu = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ContextMenu/BattlePlantContextMenu.tscn").Instantiate<BattlePlantContextMenu>();
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

        public static pause PausePanel()
        {
            pause pausePanel = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/pause.tscn").Instantiate<pause>();
            return pausePanel;
        }

        public static options OptionPanel()
        {
            options optionPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/options.tscn").Instantiate<options>();
            return optionPanel;
        }

        public static main_menu Menu()
        {
            main_menu menu = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/main_menu.tscn").Instantiate<main_menu>();
            return menu;
        }   

        public static ClassIcon ClassIcon()
        {
            ClassIcon icon = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Global/ClassIcon.tscn").Instantiate<ClassIcon>();
            return icon;
        }

        public static class EnergyItems
        {
            public static Energy Energy()
            {
                Energy energy = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Energy/Energy.tscn").Instantiate<Energy>();
                return energy;
            }
            public static EnergyContainer EnergyContainer()
            {
                EnergyContainer energyContainer = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Energy/EnergyContainer.tscn").Instantiate<EnergyContainer>();
                return energyContainer;
            }
        }

        public static class Inventory
        {
            public static InventoryWidget InventoryWidget()
            {
                InventoryWidget inventoryWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventoryWidget.tscn").Instantiate<InventoryWidget>();
                return inventoryWidget;
            }
            public static GardenInventoryWidget GardenInventoryWidget()
            {
                GardenInventoryWidget inventoryWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/GardenInventoryWidget.tscn").Instantiate<GardenInventoryWidget>();
                return inventoryWidget;
            }
            public static InventorySlot InventorySlot()
            {
                InventorySlot inventorySlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventorySlot.tscn").Instantiate<InventorySlot>();
                return inventorySlot;
            }
            public static BattlefieldBattlePlantSlot BattlefieldBattlePlantSlot()
            {
                BattlefieldBattlePlantSlot inventorySlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/BattlefieldBattlePlantSlot.tscn").Instantiate<BattlefieldBattlePlantSlot>();
                return inventorySlot;
            }
            public static InventoryAmountWindow InventoryAmountWindow()
            {
                InventoryAmountWindow inventoryAmountWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Inventory/InventoryAmountWindow.tscn").Instantiate<InventoryAmountWindow>();
                return inventoryAmountWindow;
            }
        }     

        public static class PlantTransfer
        {
            public static PlantTransferWindow PlantTransferWindow()
            {
                PlantTransferWindow window = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/PlantTransferWindow.tscn").Instantiate<PlantTransferWindow>();
                return window;
            }

            public static BattleAgentWindow BattleAgentWindow()
            {
                BattleAgentWindow window = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/BattleAgentWindow.tscn").Instantiate<BattleAgentWindow>();
                return window;
            }

            public static MonsterSlot MonsterSlot()
            {
                MonsterSlot monsterSlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/MonsterSlot.tscn").Instantiate<MonsterSlot>();
                return monsterSlot;
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
            public static MonsterTooltip MonsterTooltip()
            {
                MonsterTooltip monsterTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/MonsterTooltip.tscn").Instantiate<MonsterTooltip>();
                return monsterTooltip;
            }
            public static ClassIconTooltip ClassIconTooltip()
            {
                ClassIconTooltip classIconTooltip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/ClassIconTooltip.tscn").Instantiate<ClassIconTooltip>();
                return classIconTooltip;
            }

            public static BattlePlantTooltip BattlePlantTooltip()
            {
                BattlePlantTooltip battlePlantToolTip = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/ToolTip/BattlePlantTooltip.tscn").Instantiate<BattlePlantTooltip>();
                return battlePlantToolTip;
            }
        }

        public static class BattleWidget
        {
            public static WaveWidget WaveWidget()
            {
                WaveWidget waveWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/WaveWidget.tscn").Instantiate<WaveWidget>();
                return waveWidget;
            }

            public static WaveTimerBlock WaveTimerBlock()
            {
                WaveTimerBlock waveWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/WaveTimerBlock.tscn").Instantiate<WaveTimerBlock>();
                return waveWidget;
            }

            public static WaveCounterWidget WaveCounterWidget()
            {
                WaveCounterWidget waveWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/BattleAgent/WaveCounterWidget.tscn").Instantiate<WaveCounterWidget>();
                return waveWidget;
            }
        }

        public static class Bestiary
        {
            public static BestiaryWindow BestiaryWindow()
            {
                BestiaryWindow bestiaryWindow = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/BestiaryWindow.tscn").Instantiate<BestiaryWindow>();
                return bestiaryWindow;
            }
            public static BattlePlantDescWidget BattlePlantDescWidget()
            {
                BattlePlantDescWidget descWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/BattlePlantDescWidget.tscn").Instantiate<BattlePlantDescWidget>();
                return descWidget;
            }
            public static DescWidget DescWidget()
            {
                DescWidget descWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/DescWidget.tscn").Instantiate<DescWidget>();
                return descWidget;
            }
            public static PawnDescWidget PawnDescWidget()
            {
                PawnDescWidget descWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/PawnDescWidget.tscn").Instantiate<PawnDescWidget>();
                return descWidget;
            }
            public static PawnDescWidget MonsterDescWidget()
            {
                PawnDescWidget descWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/MonsterDescWidget.tscn").Instantiate<MonsterDescWidget>();
                return descWidget;
            }
            public static ItemDescWidget ItemDescWidget()
            {
                ItemDescWidget itemDescWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/ItemDescWidget.tscn").Instantiate<ItemDescWidget>();
                return itemDescWidget;
            }
            public static PotItemDescWidget PotItemDescWidget()
            {
                PotItemDescWidget itemDescWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/PotItemDescWidget.tscn").Instantiate<PotItemDescWidget>();
                return itemDescWidget;
            }
            public static SeedItemDescWidget SeedItemDescWidget()
            {
                SeedItemDescWidget seedItemDescWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/SeedItemDescWidget.tscn").Instantiate<SeedItemDescWidget>();
                return seedItemDescWidget;
            }
            public static FertilizerItemDescWidget FertilizerItemDescWidget()
            {
                FertilizerItemDescWidget fertilizerItemDescWidget = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/FertilizerItemDescWidget.tscn").Instantiate<FertilizerItemDescWidget>();
                return fertilizerItemDescWidget;
            }
            public static BestiaryListItem BestiaryListItem()
            {
                BestiaryListItem bestiaryListItem = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Bestiary/BestiaryListItem.tscn").Instantiate<BestiaryListItem>();
                return bestiaryListItem;
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
            public static upgrage_slot UpgradeSlot()
            {
                upgrage_slot upgrageSlot = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/upgrage_slot.tscn").Instantiate<upgrage_slot>();
                return upgrageSlot;
            }
            public static UpgradeValueEntity UpgradeValueEntity()
            {
                UpgradeValueEntity upgradeValueEntity = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/UpgradeValueEntity.tscn").Instantiate<UpgradeValueEntity>();
                return upgradeValueEntity;
            }
            public static ExpandPanel ExpandPanel()
            {
                ExpandPanel expandPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Shop/Expand/expand_panel.tscn").Instantiate<ExpandPanel>();
                return expandPanel;
            }
        }
        public static WindowConfirmation WindowConfirmation()
        {
            WindowConfirmation windowConfirmation = ResourceLoader.Load<PackedScene>("res://Scenes/Widgets/Global/WindowConfirmation.tscn").Instantiate<WindowConfirmation>();
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
        public static BattlePlantItem BattlePlantItem()
        {
            BattlePlantItem plant = ResourceLoader.Load<PackedScene>("res://Scenes/Items/BattlePlantItem.tscn").Instantiate<BattlePlantItem>();
            return plant;
        }
        public static Cell Cell()
        {
            Cell cell = ResourceLoader.Load<PackedScene>("res://Scenes/Items/Cell.tscn").Instantiate<Cell>();
            return cell;
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
    public static TowerDefenseAreaCell TowerDefenseAreaCell()
    {
        TowerDefenseAreaCell towerDefenseAreaCell = ResourceLoader.Load<PackedScene>("res://Scenes/TowerDefenseAreaCell.tscn").Instantiate<TowerDefenseAreaCell>();
        return towerDefenseAreaCell;
    }

    public static class Partials
    {
        public static BaseOneShotParticle CellPick()
        {
            BaseOneShotParticle partiacle = ResourceLoader.Load<PackedScene>("res://Particles/cell_pick.tscn").Instantiate<BaseOneShotParticle>();
            return partiacle;
        }
    }
}
