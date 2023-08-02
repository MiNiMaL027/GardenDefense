using Godot;
using System;

public partial class PlantsToolTip : Panel
{ 
	public TextureRect Icon { get; set; }
	public Label PlantsName { get; set; }
	public TextureProgressBar ProgressBar { get; set; }

	public override void _Ready()
	{
        Icon = GetNode<TextureRect>("Container/Icon");
        PlantsName = GetNode<Label>("Container/Name");
        ProgressBar = GetNode<TextureProgressBar>("Container/TextureProgressBar");
    }

	public void Init(Texture2D icon, string name,int maxStage)
	{
        GD.Print(icon);

		Icon.Texture = icon;
		PlantsName.Text = name;
		ProgressBar.MaxValue = maxStage;
		ProgressBar.Value = 1;
	}

	public void RefreshBar(int currentStage)
	{
		ProgressBar.Value = currentStage;
	}
}
