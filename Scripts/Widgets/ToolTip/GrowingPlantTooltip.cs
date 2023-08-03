using Godot;
using System;

public partial class GrowingPlantTooltip : BaseTooltip
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

	public void RefreshBar(int currentStage)
	{
		ProgressBar.Value = currentStage;
	}

    public override void ShowTooltip(Node n)
    {
        GrowingPlant plant = n as GrowingPlant;
        Icon.Texture = ResourceLoader.Load<Texture2D>(plant.SeedData.TextureSpritePath);
        PlantsName.Text = plant.SeedData.ItemName;
        ProgressBar.MaxValue = plant.SeedData.StagesAmount;
        ProgressBar.Value = plant.CurrentStage;
    }

    public override void HideTooltip()
    {
		QueueFree();
    }
}
