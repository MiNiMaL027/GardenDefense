using System;
using Components.PawnStats;
using Controllers;
using Godot;
using Pawns;
using Pawns.Monsters;

namespace Widgets.Bestiary
{
    public partial class MonsterDescWidget:PawnDescWidget
    {
        public Label LabelMovementSpeed;
        public override void _Ready()
        {
            LabelHealth = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer/LabelHealth");
            LabelDamage = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer2/LabelDamage");
            LabelAttackSpeed = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer3/LabelAttackSpeed");
            LabelRange = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/HBoxContainer4/LabelRange");

            TextureRect = GetNode<TextureRect>("VBoxContainer/HBoxContainer/TextureRect");
            LabelName = GetNode<Label>("VBoxContainer/HBoxContainer/LabelName");
            LabelDescription = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/LabelDescription");
            LabelMovementSpeed = GetNode<Label>("VBoxContainer/MarginContainer/VBoxContainer/HFlowContainer/LabelMovementSpeed");
        }
        public override void Init(object o)
        {
            if (o is PawnDatabaseRow pawnDatabaseRow)
            {
                LabelName.Text = pawnDatabaseRow.Name;
                LabelDescription.Text = pawnDatabaseRow.Description;
                TextureRect.Texture = GD.Load<Texture2D>(pawnDatabaseRow.TextureSpritePath);

                Pawn pawn = GD.Load<PackedScene>(pawnDatabaseRow.ScenePath).Instantiate<Pawn>();
                LabelRange.Text = pawn.PawnStats.AttackRange.ToString();
                LabelHealth.Text = pawn.PawnStats.MaxHealth.ToString();
                LabelDamage.Text = pawn.PawnStats.Strength.ToString();
                LabelAttackSpeed.Text = pawn.PawnStats.AttackSpeed.ToString();
                LabelMovementSpeed.Text = $"Movement speed: {(pawn.PawnStats as MonsterStats).MovementSpeed}";
                pawn.QueueFree();

            }
        }
    }
}
