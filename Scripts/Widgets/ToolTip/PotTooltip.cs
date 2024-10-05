using Godot;
using Items;
using System;
namespace Widgets.ToolTip
{
    public partial class PotTooltip : BaseTooltip
    {
        Label LabelWatered;
        Label LabelFertilizer;
        TextureRect WaterTexture;
        TextureRect FertilizerTexture;
        Timer timerRefresh;
        Pot targetPot;

        Label NameLabel;
        TextureRect IconRect;

        public override void _Ready()
        {
            base._Ready();

            NameLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/HBoxContainer3/Label");
            IconRect = GetNode<TextureRect>("MarginContainer/VBoxContainer/PanelContainer/HBoxContainer3/TextureRect");

            LabelWatered = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/LabelWatered");
            LabelFertilizer = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer2/LabelFertilizer");
            WaterTexture = GetNode<TextureRect>("MarginContainer/VBoxContainer/HBoxContainer/TextureRect");
            FertilizerTexture = GetNode<TextureRect>("MarginContainer/VBoxContainer/HBoxContainer2/TextureRect");

            timerRefresh = GetNode<Timer>("Timer");
            timerRefresh.Timeout += TimerRefresh_Timeout;
        }

        private void TimerRefresh_Timeout()
        {
            RefreshTooltip();
        }

        public void RefreshTooltip()
        {
            if (targetPot.Watered == true)
            {
                WaterTexture.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/Info/NeedWater.png");

                LabelWatered.GetParent<HBoxContainer>().Visible = true;
                LabelWatered.Text = $"{(int)targetPot.waterTimer.TimeLeft} s.";
            }
            else
            {
                WaterTexture.Texture = null;

                LabelWatered.GetParent<HBoxContainer>().Visible = false;

            }

            if (targetPot.Fertilizer != null)
            {
                FertilizerTexture.Texture = ResourceLoader.Load<Texture2D>(targetPot.Fertilizer.TextureSpritePath);

                LabelFertilizer.GetParent<HBoxContainer>().Visible = true;
                LabelFertilizer.Text = $"{(int)targetPot.fertilizeTimer.TimeLeft} s.";
            }
            else
            {
                LabelFertilizer.GetParent<HBoxContainer>().Visible = false;
            }
        }

        protected override void ViewTimer_Timeout()
        {
            Visible = true;

            RefreshTooltip();

            timerRefresh.Start();
        }

        public void ShowTooltip(Pot p)
        {
            targetPot = p;

            NameLabel.Text = p.ItemName;
            IconRect.Texture = ResourceLoader.Load<Texture2D>(p.TextureSpritePath);
        }
    }

}
