using Godot;
using Interfaces;
using System;
namespace Widgets.Shop.Upgrade
{
    public partial class upgrage_slot : Control
    {
        public VBoxContainer ValueChangeContainer;
        TextureRect Icon;
        Label NameLabel;
        Label CoinsLabel;

        public Button UpgradeButton;

        public override void _Ready()
        {
            ValueChangeContainer = GetNode<VBoxContainer>("Panel/HBoxContainer/VBoxContainer/ValueChangeContainer");
            Icon = GetNode<TextureRect>("Panel/HBoxContainer/TextureRect");
            NameLabel = GetNode<Label>("Panel/HBoxContainer/VBoxContainer/Name");
            CoinsLabel = GetNode<Label>("Panel/HBoxContainer/Button/HBoxContainer/Coins");

            UpgradeButton = GetNode<Button>("Panel/HBoxContainer/Button");
        }

        public void Init(string name, int coinsCount, Texture2D iconTexture, Action buttonPressed)
        {
            NameLabel.Text = name;
            Icon.Texture = iconTexture;
            CoinsLabel.Text = coinsCount.ToString();

            UpgradeButton.Pressed += buttonPressed;
        }

        public void Refresh(IUpgradable itemToUpgrade)
        {
            CoinsLabel.Text = itemToUpgrade.CostToUpgrade.ToString();

            if (this.GetPlayerController().Gold >= itemToUpgrade.CostToUpgrade)
            {
                CoinsLabel.LabelSettings.FontColor = new Color(0.20f, 0.95f, 0.00f);
            }
            else
            {
                CoinsLabel.LabelSettings.FontColor = new Color(0.651f, 0.086f, 0.059f);
            }

            if (itemToUpgrade.CountOfAvalibalUpgrades == 0)
            {
                UpgradeButton.Icon = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/6.png");

                foreach (UpgradeValueEntity value in ValueChangeContainer.GetChildren())
                {
                    value.Block();
                }
            }
            else
            {
                UpgradeButton.Icon = default;

                foreach (UpgradeValueEntity value in ValueChangeContainer.GetChildren())
                {
                    value.UnBlock();
                }
            }
        }
    }

}
