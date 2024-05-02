using Godot;
using System;

namespace Widgets.Shop.Upgrade
{
    public class UpgradeService
    {
        VBoxContainer SlotContainer;
        Funnel funnel;

        upgrage_slot funnelSlot;

        public event EventHandler Refresh;

        public void Init(VBoxContainer container)
        {
            SlotContainer = container;
            if (GameInstance.World is Farm f)
            {
                funnel = f.Funnel;
                AddFunnel();
            }
        }
        public void AddFunnel()
        {
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

            Refresh?.Invoke(this, EventArgs.Empty);
        }

    }
}
