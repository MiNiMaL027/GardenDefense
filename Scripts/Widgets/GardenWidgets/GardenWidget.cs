using Godot;
using Widgets.Inventory;
using Controllers;
using Widgets.BattleAgent;

namespace Widgets.GardenWidgets
{
    public partial class GardenWidget : MainWidget
    {
        public InventoryWidget InventoryWidget { get; set; }
        public BattleAgentWindow BattleAgentWindow { get; set; }
        public Button FightButton { get; set; }
        public Label LabelGold { get; set; }
        public HBoxContainer CoinContainer { get; set; }
        public AnimationPlayer CoinAnim { get; set; }
        public Timer CoinVisualizeTimer { get; set; }
        public int CoinVisualizeTime { get; set; } = 5;
        public override void _Ready()
        {
            base._Ready();
            LabelGold = GetNode<Label>("HBoxContainer/LabelGold");
            CoinContainer = GetNode<HBoxContainer>("HBoxContainer");
            CoinAnim = GetNode<AnimationPlayer>("HBoxContainer/CoinAnim");

            CoinVisualizeTimer = new Timer();
            AddChild(CoinVisualizeTimer);
            CoinVisualizeTimer.WaitTime = CoinVisualizeTime;
            CoinVisualizeTimer.Timeout += CoinVisualizeTimer_Timeout;
            CoinVisualizeTimer.OneShot = true;

            this.GetPlayerController().GoldChange += UpdateGold;
            FightButton = GetNode<Button>("FightContainer/FightButton");
            FightButton.Pressed += FightButton_Pressed;
        }
        private void CoinVisualizeTimer_Timeout()
        {
            CoinAnim.Play("Capasity");
        }

        public override void Init(PlayerController playerController)
        {
            UpdateGold(playerController.Gold);
        }

        public void UpdateGold(int newGold)
        {
            CoinContainer.Visible = true;
            CoinContainer.Modulate = new Color(1, 1, 1, 1);
            LabelGold.Text = newGold.ToString();

            CoinVisualizeTimer.Start(0);
        }
        private void FightButton_Pressed()
        {
             OpenBattleAgentWindow();
        }
        public override void OpenInventory()
        {
            InventoryWidget = Scenes.Widgets.Inventory.InventoryWidget();

            AddChild(InventoryWidget);
            InventoryWidget.SetInventory(this.GetPlayerController().GardenInventory);
        }

        public override void CloseInventory()
        {
            PlayerController playerController = this.GetPlayerController();
            if (playerController.OpenedContextMenu != null && playerController.OpenedContextMenu.isInventorySlot)
                playerController.RemoveOpenedContextMenu();

            InventoryWidget.QueueFree();
            
            InventoryWidget = null;
        }

        public void OpenBattleAgentWindow()
        {
            BattleAgentWindow = Scenes.Widgets.PlantTransfer.BattleAgentWindow();

            AddChild(BattleAgentWindow);
        }

        public void CloseBattleAgentWindow()
        {
            BattleAgentWindow.QueueFree();

            BattleAgentWindow = null;
        }

        public override void ToggleInventory()
        {
            if(InventoryWidget != null)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        public override void _ExitTree()
        {
            base._ExitTree();
            this.GetPlayerController().GoldChange -= UpdateGold;

        }
    }
}
