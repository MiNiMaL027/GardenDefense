using Godot;

public partial class WaveWidget : PanelContainer
{
	Label NumberLabel;
	public override void _Ready()
	{
		NumberLabel = GetNode<Label>("MarginContainer/HBoxContainer/NumberLabel");
	}

	public void Init(string waveNumber)
	{
		NumberLabel.Text = waveNumber;
		this.FadeOutControl(2);
	}
}
