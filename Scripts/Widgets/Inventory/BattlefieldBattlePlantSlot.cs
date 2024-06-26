using Controllers;
using Enums;
using Godot;
using Items;
using System;
using System.ComponentModel;
using Widgets.ContextMenu;
using Widgets.ToolTip;

namespace Widgets.Inventory
{
    public partial class BattlefieldBattlePlantSlot : BaseSlot
    {
        public bool IsBlocked = false;
        Item item;
        ShaderMaterial material;
        Label LabelEnergy;
        Timer TimerBlockSlot;
        BattlePlantDataBaseRow battlePlantDataBaseRow;
        public override void _Ready()
        {
            SetProcess(false);
            TextureRect = GetNode<TextureRect>("Panel/TextureRect");
            LabelAmount = GetNode<Label>("Panel/LabelAmount");
            material = (ShaderMaterial)GetNode<ColorRect>("Panel/ShaderColorRect").Material;
            TimerBlockSlot = GetNode<Timer>("TimerBlockSlot");
            TimerBlockSlot.Timeout += TimerBlockSlot_Timeout;
            LabelEnergy = GetNode<Label>("HBoxContainer/LabelEnergy");
            base._Ready();
        }

        private void TimerBlockSlot_Timeout()
        {
            IsBlocked = false;
            SetProcess(false);

        }

        public override void _Process(double delta)
        {
            material.SetShaderParameter("cutoff", TimerBlockSlot.TimeLeft / TimerBlockSlot.WaitTime);
        }
        public override void _GuiInput(InputEvent e)
        {
            base._GuiInput(e);
            if (e is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left && mouseButton.IsPressed() == true && IsBlocked == false)
            {
                PlayerController playerController = this.GetPlayerController();
                ItemType itemType = DbService.GetItemType(battlePlantDataBaseRow.Id);
                item = Item.GetItemSceneByType(itemType);
                if (playerController.BattlefieldEnergy < battlePlantDataBaseRow.EnergyToPlace)
                {
                    playerController.Hud.MainWidget.InfoWindow.AddInfoPanel($"Required {battlePlantDataBaseRow.EnergyToPlace} energy to use plant");
                    return;
                }
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
            LabelEnergy.Text = battlePlantDataBaseRow.EnergyToPlace.ToString();
            this.GetPlayerController().EnergyUpdated += BattlefieldBattlePlantSlot_EnergyUpdated;
        }

        private void BattlefieldBattlePlantSlot_EnergyUpdated(int newEnergy)
        {
            if (newEnergy >= battlePlantDataBaseRow.EnergyToPlace)
            {
                LabelEnergy.Set("theme_override_colors/font_color", Color.Color8(255, 255, 255));

            }
            else 
            {
                LabelEnergy.Set("theme_override_colors/font_color", Color.Color8(255, 0, 0));
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
            IsBlocked = true;
        }
    }
}
