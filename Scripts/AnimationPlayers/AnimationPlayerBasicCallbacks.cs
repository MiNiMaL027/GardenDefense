using Godot;
using System;

public partial class AnimationPlayerBasicCallbacks : AnimationPlayer
{
    [Signal]
    public delegate void ProjectileSpawnEventHandler();
    [Signal]
    public delegate void AttackEndedEventHandler();
    [Signal]
    public delegate void AttackStartedEventHandler();
    public override void _Ready()
	{
	}
    public void ProjectileSpawnListener()
    {
        EmitSignal(SignalName.ProjectileSpawn);
    }
    public void AttackEndedListener()
    {
        EmitSignal(SignalName.AttackEnded);
    }
    public void AttackStartedListener()
    {
        EmitSignal(SignalName.AttackEnded);
    }
}
