using Farm.Scripts.Enums;
using Farm.Scripts.Interfaces;
using Godot;

public partial class BaseProjectile : Node3D, IAttacking
{
	const int CellWight = 1;
	public AnimationPlayer Animation { get; set; }
	public Node3D MeshSpace { get; set; }
	public DamageArea DamageArea { get; set; }
    public int Damage { get; set; }
	public int Distance { get; set; }
    public AttackModify AttackModify { get; set; }
	Tween Tween { get; set; }

    public override void _Ready()
	{
		Animation = GetNode<AnimationPlayer>("Animation");
		MeshSpace = GetNode<Node3D>("MeshNode");
	}

	public void Init(string meshPath, int damage, AttackModify type, int attackRange)
	{
		var mesh = ResourceLoader.Load<MeshInstance3D>(meshPath);
		MeshSpace.AddChild(mesh);
		Damage = damage;
		AttackModify = type;
		Distance = attackRange * CellWight;

		DamageArea = mesh.GetChild<DamageArea>(0);
		DamageArea.IsProjectile = true;

        Tween = CreateTween();

	}

	public void Launch()
	{
		Tween.TweenProperty(this, "position", Position.X, Distance);

        Tween.Finished += Tween_Finished;
	}

    private void Tween_Finished()
    {
		Animation.Play("Fall");

        Animation.AnimationFinished += Animation_AnimationFinished;
    }

    private void Animation_AnimationFinished(StringName animName)
    {
		if (animName == "Fall")
			QueueFree();
    }
}
