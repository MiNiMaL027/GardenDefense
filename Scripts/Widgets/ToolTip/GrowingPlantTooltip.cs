using Godot;
using System;
namespace Widgets.ToolTip
{
    public partial class GrowingPlantTooltip : BaseTooltip
    {
        public TextureRect Icon { get; set; }
        public Label PlantsName { get; set; }
        private HBoxContainer ProgressBarContainer { get; set; }

        int MaxProgresBarValue;
        public int CurentProgresBarValue;

        private Vector2 screenSize;

        public override void _Ready()
        {
            timeToView = 1;

            base._Ready();

            Icon = GetNode<TextureRect>("MarginContainer/Container/Panel/VBoxContainer/Icon");
            PlantsName = GetNode<Label>("MarginContainer/Container/Panel/VBoxContainer/Name");

            ProgressBarContainer = GetNode<HBoxContainer>("MarginContainer/Container/Panel2/MarginContainer/HBoxContainer");

            screenSize = GetWindow().Size;
        }

        public void RefreshBar(int currentStage)
        {
            for (int i = 0; i < currentStage; i++)
            {
                var textureRect = ProgressBarContainer.GetChild<TextureRect>(i);

                if (i == 0)
                {
                    textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/StartFullProgressBarl.png");
                    continue;
                }
                else if (i == MaxProgresBarValue - 1)
                {
                    textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/FinishFullProgressBarl.png");
                    continue;
                }

                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/MidFullProgressBarl.png");
            }
        }

        public void ShowTooltip(GrowingPlant plant)
        {
            Icon.Texture = ResourceLoader.Load<Texture2D>(plant.SeedData.TextureSpritePath);
            PlantsName.Text = plant.SeedData.ItemName;
            CurentProgresBarValue = plant.CurrentStage;

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

                if (i == 0)
                {
                    textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/StartFullProgressBarl.png");
                    ProgressBarContainer.AddChild(textureRect);
                    continue;
                }
                else if (i == count - 1)
                {
                    textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/FinishEmptyProgressBarl-sheet.png");
                    ProgressBarContainer.AddChild(textureRect);
                    continue;
                }

                textureRect.Texture = ResourceLoader.Load<Texture2D>("res://raw assets/Images/ToolsButton/ProgresBar/MidEmptyProgressBarl-sheet.png");

                ProgressBarContainer.AddChild(textureRect);
            }

            RefreshBar(CurentProgresBarValue);
        }

        private void ClearProgresBar()
        {
            foreach (TextureRect child in ProgressBarContainer.GetChildren())
            {
                ProgressBarContainer.RemoveChild(child);

                child.QueueFree();
            }
        }
    }

}
