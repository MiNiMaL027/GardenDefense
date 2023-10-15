using Farm.Scripts.Enums;
using Farm.Scripts.Interfaces;
using Godot;
using System;

public partial class BaseProjectile : Node3D, IAttacking
{
	public AnimationPlayer Animation { get; set; }
	public Node3D MeshSpace { get; set; }
	public DamageArea DamageArea { get; set; }
    public int Damage { get; set; }
    public AttackType AttackType { get; set; }

    public override void _Ready()
	{
		Animation = GetNode<AnimationPlayer>("Animation");
		MeshSpace = GetNode<Node3D>("MeshNode");	
	}

	public void Init(string meshPath, int damage, AttackType type)
	{
		var mesh = ResourceLoader.Load<MeshInstance3D>(meshPath);
		MeshSpace.AddChild(mesh);
		Damage = damage;
		AttackType = type;

		DamageArea = mesh.GetChild<DamageArea>(0);
		DamageArea.IsProjectile = true;
	}
}
