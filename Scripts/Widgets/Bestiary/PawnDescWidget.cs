using System;
using Components;
using Controllers;
using Godot;
using Pawns;

namespace Widgets.Bestiary
{
    public partial class PawnDescWidget:DescWidget
    {
        public Label LabelHealth;
        public Label LabelDamage;
        public Label LabelAttackSpeed;
        public Label LabelRange;


        public override void _Ready()
        {
            LabelHealth = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer/LabelHealth");
            LabelDamage = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer2/LabelDamage");
            LabelAttackSpeed = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer3/LabelAttackSpeed");
            LabelRange = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer4/LabelRange");

            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");
        }
        public override void Init(object o)
        {
            if(o is PawnDatabaseRow pawnDatabaseRow)
            {
                LabelName.Text = pawnDatabaseRow.Name;
                LabelDescription.Text = pawnDatabaseRow.Description;
                TextureRect.Texture = GD.Load<Texture2D>(pawnDatabaseRow.TextureSpritePath);

                AIController aiController = GD.Load<PackedScene>(pawnDatabaseRow.DefaultAIScenePath).Instantiate<AIController>();
                LabelRange.Text=aiController.AttackRange.ToString();
                aiController.BestiaryReady();

                LabelHealth.Text=aiController.Pawn.StatsComponent.GetMaxHealth().ToString();
                LabelDamage.Text = aiController.Pawn.StatsComponent.GetStrength().ToString();
                LabelAttackSpeed.Text = aiController.Pawn.AttackSpeed.ToString();

                aiController.QueueFree();
                
            }
        }
    }
}
