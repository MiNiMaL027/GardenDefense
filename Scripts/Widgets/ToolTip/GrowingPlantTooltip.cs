using Farm.Scripts.Widgets.ToolTip;
using Godot;
using System;

public partial class GrowingPlantTooltip : BaseTooltip
{ 
	public TextureRect Icon { get; set; }
	public Label PlantsName { get; set; }
    private HBoxContainer ProgressBarContainer { get; set; }

    int MaxProgresBarValue;

    private Vector2 screenSize;

    public override void _Ready()
    {
        timeToView = 1;
        base._Ready();
        Icon = GetNode<TextureRect>("Panel/Icon");
        PlantsName = GetNode<Label>("Name");

        ProgressBarContainer = GetNode<HBoxContainer>("HBoxContainer");

        screenSize = GetWindow().Size;
    }

    public void RefreshBar(int currentStage)
	{
        for (int i = 0; i < currentStage; i++)
        {
            var textureRect = ProgressBarContainer.GetChild<TextureRect>(i);

            if(i == 0)
            {
                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Full_1.png");
                continue;
            }
            else if(i == MaxProgresBarValue - 1)
            {
                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Full_3.png");
                continue;
            }

            textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Full_2.png");
        }
	}

    public void ShowTooltip(GrowingPlant plant)
    {   
        Icon.Texture = ResourceLoader.Load<Texture2D>(plant.SeedData.TextureSpritePath);
        PlantsName.Text = plant.SeedData.ItemName;

        GenerateProgresBar(plant.SeedData.StagesAmount);      
    }

    private void GenerateProgresBar(int count)
    {
        MaxProgresBarValue = count;
        ClearProgresBar();

        for (int i = 0; i < count; i++)
        {
            var textureRect = new TextureRect();
            textureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
          
            if(i == 0)
            {
                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Full_1.png");
                ProgressBarContainer.AddChild(textureRect);
                continue;
            }
            else if (i == count - 1)
            {           
                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Empty_2.png");
                ProgressBarContainer.AddChild(textureRect);
                continue;
            }

            GD.Print(i);
            textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/Empty_1.png");
            ProgressBarContainer.AddChild(textureRect);

            RefreshBar(1);
        }
    }

    private void ClearProgresBar()
    {
        foreach(TextureRect child in ProgressBarContainer.GetChildren())
        {
            child.QueueFree();
        }
    }
}
