using Godot;
using Widgets.Inventory;
using Controllers;
using Widgets.Global;
using Enums;

namespace Widgets.GardenWidgets
{
    public partial class GardenWidget : Control
    {
        public InventoryWidget InventoryWidget { get; set; }
        public InfoWindow InfoWindow { get; set; }
        public WorldTimer WorldTimer { get; set; }
        public BattleAgentWindow BattleAgentWindow { get; set; }

        Label LabelGold { get; set; }
        HBoxContainer CoinContainer { get; set; }
        AnimationPlayer CoinAnim { get; set; }
        Timer CoinVisualizeTimer { get; set; }
        int CoinVisualizeTime { get; set; } = 5;

        public Button FightButton { get; set; }

        public override void _Ready()
        {
            WorldTimer = GetNode<WorldTimer>("WorldTimer");
            WorldTimer.Init(WorldTimerMode.CountDown, 3);
            CoinVisualizeTimer = new Timer();
            AddChild(CoinVisualizeTimer);
            CoinVisualizeTimer.WaitTime = CoinVisualizeTime;
            CoinVisualizeTimer.Timeout += CoinVisualizeTimer_Timeout;
            CoinVisualizeTimer.OneShot = true;

            LabelGold = GetNode<Label>("HBoxContainer/LabelGold");
            InfoWindow = GetNode<InfoWindow>("InfoWindow");
            CoinContainer = GetNode<HBoxContainer>("HBoxContainer");
            CoinAnim = GetNode<AnimationPlayer>("HBoxContainer/CoinAnim");
            FightButton = GetNode<Button>("FightContainer/FightButton");

            FightButton.Pressed += FightButton_Pressed;
        }

        private void FightButton_Pressed()
        {
             OppenBattleAgentWindow();
        }

        private void CoinVisualizeTimer_Timeout()
        {
            CoinAnim.Play("Capasity");
        }

        internal void Init(PlayerController playerController)
        {
            UpdateGold(playerController.Gold);
        }

        public void UpdateGold(int newGold)
        {
            CoinContainer.Visible = true;
            LabelGold.Text = newGold.ToString();

            CoinVisualizeTimer.Start();
        }

        public void OpenInventory()
        {
            InventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(InventoryWidget);
            InventoryWidget.SetInventory(this.GetPlayerController().MainInventory);
        }

        public void CloseInventory()
        {
            if (this.GetPlayerController().OpenedContextMenu != null && this.GetPlayerController().OpenedContextMenu.isInventorySlot)
                this.GetPlayerController().RemoveOpenedContextMenu();

            InventoryWidget.QueueFree();
            
            InventoryWidget = null;
        }

        public void OppenBattleAgentWindow()
        {
            BattleAgentWindow = Scenes.Widgets.PlantTransfer.BattleAgentWindow();

            AddChild(BattleAgentWindow);
        }

        public void CloseBattleAgentWindow()
        {
            BattleAgentWindow.QueueFree();

            BattleAgentWindow = null;
        }
    }
}
