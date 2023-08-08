using Godot;
using System;

public partial class PotTooltip : Control
{
    Label LabelWatered;
    Label LabelFertilizer;
    TextureRect WaterTexture;
    TextureRect FertilizerTexture;
    Timer timerRefresh;
    Pot targetPot;
    public override void _Ready()
	{
        LabelWatered = GetNode<Label>("VBoxContainer/HBoxContainer/LabelWatered");
        LabelFertilizer = GetNode<Label>("VBoxContainer/HBoxContainer2/LabelFertilizer");
        WaterTexture = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
        FertilizerTexture = GetNode<TextureRect>("VBoxContainer/HBoxContainer2/TextureRect");

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
    public void ShowTooltip(Pot p)
    {
        targetPot= p;
        RefreshTooltip();
        timerRefresh.Start();
    }
}
