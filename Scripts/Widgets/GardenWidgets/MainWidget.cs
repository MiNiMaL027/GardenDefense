using Controllers;
using Godot;
using Widgets.Global;
using Widgets.Inventory;

namespace Widgets.GardenWidgets
{
    public partial class MainWidget :Control
    {
        public InfoWindow InfoWindow { get; set; }
        public Label LabelGold { get; set; }
        public HBoxContainer CoinContainer { get; set; }
        public AnimationPlayer CoinAnim { get; set; }
        public Timer CoinVisualizeTimer { get; set; }
        public int CoinVisualizeTime { get; set; } = 5;
        public override void _Ready()
        {
            LabelGold = GetNode<Label>("HBoxContainer/LabelGold");
            InfoWindow = GetNode<InfoWindow>("InfoWindow");
            CoinContainer = GetNode<HBoxContainer>("HBoxContainer");
            CoinAnim = GetNode<AnimationPlayer>("HBoxContainer/CoinAnim");

            CoinVisualizeTimer = new Timer();
            AddChild(CoinVisualizeTimer);
            CoinVisualizeTimer.WaitTime = CoinVisualizeTime;
            CoinVisualizeTimer.Timeout += CoinVisualizeTimer_Timeout;
            CoinVisualizeTimer.OneShot = true;
        }
        private void CoinVisualizeTimer_Timeout()
        {
            CoinAnim.Play("Capasity");
        }
        public virtual void Init(PlayerController playerController)
        {
            UpdateGold(playerController.Gold);
        }
        public void UpdateGold(int newGold)
        {
            CoinContainer.Visible = true;
            LabelGold.Text = newGold.ToString();

            CoinVisualizeTimer.Start();
        }
        public virtual void OpenInventory() { }
        public virtual void ToggleInventory() { }


        public virtual void CloseInventory() { }
    }
}
