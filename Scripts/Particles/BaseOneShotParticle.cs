using Godot;

namespace Particles
{
    public partial class BaseOneShotParticle : GpuParticles3D
    {
        public override void _Ready()
        {
            base._Ready();
            Emitting = true;
            Finished += QueueFree;
        }
    }
}
