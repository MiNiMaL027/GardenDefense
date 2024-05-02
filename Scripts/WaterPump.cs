using BaseClasses;
using Godot;
using Interfaces;

public partial class WaterPump : BaseStaticBody3D, IHoverable
{
	public Vector3 funnelPosition;

	AnimationPlayer Animation;
	Funnel currentFunnel;
	GpuParticles3D particles;

	public override void _Ready()
	{
		funnelPosition = GetNode<PinJoint3D>("FunnelPosition").GlobalPosition;
		Animation = GetNode<AnimationPlayer>("Water_pump/AnimationPlayer");
		particles = GetNode<GpuParticles3D>("Particle");

		mesh = GetNode<MeshInstance3D>("Water_pump/sceleton/Skeleton3D/Цилиндр");
	}

    public void FillFunnel(Funnel funnel)
	{
		currentFunnel = funnel;

		funnel.SetDeferred("global_position", funnelPosition);
		funnel.isInteractable = false;
		funnel.Freeze = true;

		Animation.Play("Action_001");
	}

	public void FinishFill()
	{
		currentFunnel.Freeze = false;
		currentFunnel.isInteractable = true;

		currentFunnel.FillWithWater();
	}
}
