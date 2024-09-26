using Controllers;
using Enums;
using Godot;
using Items;
using System;

namespace Widgets.Inventory
{
    public partial class BattlefieldBattlePlantSlot : BaseSlot
    {
        Item item;
        ShaderMaterial material;
        Label LabelEnergy;
        Timer TimerBlockSlot;
        BattlePlantDataBaseRow battlePlantDataBaseRow;
        HBoxContainer EnergyContainer;

        SlotBlocker SlotBlockers = SlotBlocker.None;
        public override void _Ready()
        {
            SetProcess(false);
            TextureRect = GetNode<TextureRect>("Panel/TextureRect");
            LabelAmount = GetNode<Label>("Panel/LabelAmount");
            EnergyContainer = GetNode<HBoxContainer>("Panel/EnergyContainer");
            material = (ShaderMaterial)GetNode<ColorRect>("Panel/ShaderColorRect").Material;
            TimerBlockSlot = GetNode<Timer>("TimerBlockSlot");
            TimerBlockSlot.Timeout += TimerBlockSlot_Timeout;
            base._Ready();
        }

        private void TimerBlockSlot_Timeout()
        {
            SlotBlockers &= ~SlotBlocker.Cooldown;

            SetProcess(false);
        }

        public override void _Process(double delta)
        {
            material.SetShaderParameter("cutoff", TimerBlockSlot.TimeLeft / TimerBlockSlot.WaitTime);
        }
        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);
            if (e is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsPressed() == true)
            {
                PlayerController playerController = this.GetPlayerController();
                if (SlotBlockers != SlotBlocker.None)
                {
                    Disable(1, new Color(0.608f, 0.118f, 0, 0.5f), new Color(1, 1, 1, 1));

                    if ((SlotBlockers & SlotBlocker.EnergyOut) == SlotBlocker.EnergyOut)
                    {
                        playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"Required {battlePlantDataBaseRow.EnergyToPlace} energy to use plant", battlePlantDataBaseRow.TextureSpritePath);
                    }

                    if ((SlotBlockers & SlotBlocker.Cooldown) == SlotBlocker.Cooldown)
                    {
                        playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"Plant on cooldown", battlePlantDataBaseRow.TextureSpritePath);
                    }

                    return;
                }
            
                ItemType itemType = DbService.GetItemType(battlePlantDataBaseRow.Id);
                item = Item.GetItemSceneByType(itemType);
                
                playerController.BattlefieldEnergy -= battlePlantDataBaseRow.EnergyToPlace;

                ///spawn item in world and make it current pressed object
                Node ownerParent = playerController.GetParent();

                ownerParent.AddChild(item);
                ownerParent.MoveChild(item, playerController.GetIndex());

                item.GlobalPosition = playerController.CameraBase.GlobalPosition + new Vector3(7, 0, 3) + playerController.CameraBase.GlobalTransform.Basis.Y * 2;
                item.InitializeItem(battlePlantDataBaseRow);

                playerController.CurrentPressedObject = item;
                playerController.CurrentPressedObject.LeftMouseDownListener(mouseButton, playerController);
                this.FindParentOfType<InventoryWidget>().InventoryComponent.RemoveItem(ItemDatabaseRow.Id, 1);
                BlockSlot();
            }
            else if (e is InputEventMouseButton mouseButtonUp && mouseButtonUp.ButtonIndex == MouseButton.Left && mouseButtonUp.IsPressed() == false)
            {
                mouseButtonUp.GlobalPosition = GetViewport().GetMousePosition();
                PlayerController playerController = this.GetPlayerController();

                playerController._UnhandledInput(mouseButtonUp);

                if(item != null && GodotObject.IsInstanceValid(item))
                {
                    item.LinearVelocity = Vector3.Zero;
                }
            }
        }
        public override void Init(ItemDatabaseRow item, int amountToSet)
        {
            base.Init(item, amountToSet);
            battlePlantDataBaseRow = (BattlePlantDataBaseRow)item;
            TimerBlockSlot.WaitTime = battlePlantDataBaseRow.PlacementCooldown;
            var energyTexture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Hud/Energy_full.png");
            
            EnergyContainer.RemoveChildren();
            for (int i = 0; i < battlePlantDataBaseRow.EnergyToPlace; i++)
            {
                var textureRect = new TextureRect() { Texture = energyTexture, ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional };
                EnergyContainer.AddChild(textureRect);
            }

            BattlefieldBattlePlantSlot_EnergyUpdated(this.GetPlayerController().BattlefieldEnergy);
            this.GetPlayerController().EnergyUpdated += BattlefieldBattlePlantSlot_EnergyUpdated;           
        }

        private void BattlefieldBattlePlantSlot_EnergyUpdated(int newEnergy)
        {
            EnergyContainer.RemoveChildren();
            var yellowEnergy = battlePlantDataBaseRow.EnergyToPlace;
            var redEnergy = Math.Max(battlePlantDataBaseRow.EnergyToPlace - newEnergy, 0);
            yellowEnergy -= redEnergy;
            var energyTexture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Hud/Energy_full.png");

            for (int i = 0; i < yellowEnergy; i++)
            {
                var textureRect = new TextureRect() { Texture = energyTexture, ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional };
                EnergyContainer.AddChild(textureRect);
            }
            for (int i = 0; i < redEnergy; i++)
            {
                var textureRect = new TextureRect() { Texture = energyTexture, ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional, Modulate = new Color(0.608f, 0.09f, 0) };
                EnergyContainer.AddChild(textureRect);
            }

            if (newEnergy < battlePlantDataBaseRow.EnergyToPlace)
            {
                SlotBlockers |= SlotBlocker.EnergyOut;
            }
            else
            {
                SlotBlockers &= ~SlotBlocker.EnergyOut;
            }         
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            this.GetPlayerController().EnergyUpdated -= BattlefieldBattlePlantSlot_EnergyUpdated;

        }
        public void BlockSlot()
        {
            TimerBlockSlot.Start();
            SetProcess(true);
            SlotBlockers |= SlotBlocker.Cooldown;
        }

        private void Disable(int disabledTime, Color disableColor, Color defaultColor)
        {
            Tween tween = CreateTween();
            Modulate = disableColor;
            tween.TweenProperty(this, "modulate", defaultColor, disabledTime);
        }
    }
}
