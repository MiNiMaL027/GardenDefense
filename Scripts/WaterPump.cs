using Godot;

public partial class WaterPump : StaticBody3D
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
	}

    public void FillFunnel(Funnel funnel)
	{
		currentFunnel = funnel;

		funnel.SetDeferred("global_position", funnelPosition);
		funnel.isInteractable = false;
		funnel.Freeze = true;

		Animation.Play("Action_001");
		particles.Emitting = true;
	}

	public void FinishFill()
	{
		currentFunnel.Freeze = false;
		currentFunnel.isInteractable = true;

		particles.Emitting = false;
		currentFunnel.FillWithWater();
	}
}
