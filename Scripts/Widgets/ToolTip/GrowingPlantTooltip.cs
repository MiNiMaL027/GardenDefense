using Farm.Scripts.Widgets.ToolTip;
using Godot;
using System;

public partial class GrowingPlantTooltip : BaseTooltip
{ 
	public TextureRect Icon { get; set; }
	public Label PlantsName { get; set; }
	public TextureProgressBar ProgressBar { get; set; }

    private Vector2 screenSize;

    public override void _Ready()
    {
        timeToView = 1;
        base._Ready();
        Icon = GetNode<TextureRect>("Icon");
        PlantsName = GetNode<Label>("Name");
        ProgressBar = GetNode<TextureProgressBar>("TextureProgressBar");

        screenSize = GetWindow().Size;

        GD.Print(screenSize);
    }

    public void RefreshBar(int currentStage)
	{
		ProgressBar.Value = currentStage;
	}

    public void ShowTooltip(GrowingPlant plant)
    {   
        Icon.Texture = ResourceLoader.Load<Texture2D>(plant.SeedData.TextureSpritePath);
        PlantsName.Text = plant.SeedData.ItemName;
        ProgressBar.MaxValue = plant.SeedData.StagesAmount;
        ProgressBar.Value = plant.CurrentStage;
    }
}
