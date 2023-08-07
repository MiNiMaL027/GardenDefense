using Godot;
using System;

public partial class PotTooltip : BaseTooltip
{
    Label LabelWatered;
    Label LabelFertilizer;
    Timer timerRefresh;
    Pot targetPot;
    public override void _Ready()
	{
        LabelWatered = GetNode<Label>("PanelContainer/VBoxContainer/LabelWatered");
        LabelFertilizer = GetNode<Label>("PanelContainer/VBoxContainer/LabelFertilizer");
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
            LabelWatered.Visible = true;
            LabelWatered.Text = $"Watered: {(int)targetPot.waterTimer.TimeLeft} s.";
        }
        else
        {
            LabelWatered.Visible = false;

        }

        if (targetPot.Fertilizer != null)
        {
            LabelFertilizer.Visible = true;
            LabelFertilizer.Text = $"{targetPot.Fertilizer.ItemName}: {(int)targetPot.fertilizeTimer.TimeLeft} s.";
        }
        else
        {
            LabelFertilizer.Visible = false;
        }
    }
    public override void ShowTooltip(Node n)
    {
        Pot p = n as Pot;
        targetPot= p;
        RefreshTooltip();
        timerRefresh.Start();
    }

    public override void HideTooltip()
    {
        QueueFree();
    }
}
