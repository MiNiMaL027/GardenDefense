using Godot;
using System;

namespace Farm.Scripts.Widgets.Shop.Upgrade
{
    public partial class UpgradeService : Node
    {
        VBoxContainer SlotContainer;

        upgrage_slot funnelSlot;

        [Signal]
        public delegate void RefreshEventHandler();

        public void Init(VBoxContainer container)
        {
            SlotContainer = container;

            AddFunnel();
        }

        public void AddFunnel()
        {
            var funnel = GameInstance.World.Funnel;
            var slot = Scenes.Widgets.Shop.UpgradeSlot();
            funnelSlot = slot;
            SlotContainer.AddChild(slot);

            Action Methods = funnel.Upgrade;
            Methods += FunnelSlot_Refresh;

            slot.Init("Funnel",funnel.CostToUpgrade, ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png"), Methods);
            var funnerValue1 = Scenes.Widgets.Shop.UpgradeValueEntity();
            slot.ValueChangeContainer.AddChild(funnerValue1);
            funnerValue1.Init("MaxCountWater", funnel.maxNumberOfWater, funnel.maxNumberOfWater + 2);

            slot.Refresh(funnel);
        }

        private void FunnelSlot_Refresh()
        {
            var funnel = GameInstance.World.Funnel;
            funnelSlot.Refresh(funnel);
            funnelSlot.ValueChangeContainer.GetChild<UpgradeValueEntity>(0).Refresh(funnel.maxNumberOfWater, funnel.maxNumberOfWater + 2);

            if (funnel.CountOfAvalibalUpgrades == 0)
            {
                funnelSlot.UpgradeButton.Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/6.png");
                funnelSlot.ValueChangeContainer.GetChild<UpgradeValueEntity>(0).Block();
            }
            else
            {
                funnelSlot.UpgradeButton.Icon = default;
                funnelSlot.ValueChangeContainer.GetChild<UpgradeValueEntity>(0).UnBlock();
            }

            EmitSignal(SignalName.Refresh);
        }
    }
}
