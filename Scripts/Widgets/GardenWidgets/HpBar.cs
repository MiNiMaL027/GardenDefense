using Godot;
using System;

public partial class HpBar : Control
{
	ProgressBar ProgressBar { get; set; }
	Label CurrentHpLabel { get; set; }
	Label MaxHpLabel { get; set; }
    AnimationPlayer Animation { get; set; }

    Timer VisibleTimer { get; set; }
    int visibleTime = 3;

    public override void _Ready()
	{
		ProgressBar = GetNode<ProgressBar>("VBoxContainer2/VBoxContainer/ProgressBar");
		CurrentHpLabel = GetNode<Label>("VBoxContainer2/CurrentHp");
		MaxHpLabel = GetNode<Label>("VBoxContainer2/VBoxContainer/MaxHp");
        Animation = GetNode<AnimationPlayer>("Animation");

        VisibleTimer = new Timer();
        AddChild(VisibleTimer);
        VisibleTimer.WaitTime = visibleTime;
        VisibleTimer.Autostart = false;
        VisibleTimer.OneShot = true;

        VisibleTimer.Timeout += VisibleTimer_Timeout;
    }

    private void VisibleTimer_Timeout()
    {
        Animation.Play("Hide");
    }

    public void Refresh(int currentHp,int maxHp)
	{
        VisibleTimer.Start(0);
        Modulate = new Color(1, 1, 1, 1);

        CurrentHpLabel.Text = currentHp.ToString();
		MaxHpLabel.Text = maxHp.ToString();

		ProgressBar.Value = currentHp;
		ProgressBar.MaxValue = maxHp;
	}
}
