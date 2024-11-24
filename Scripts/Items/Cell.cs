using BaseClasses;
using Controllers;
using Godot;
using Interfaces;

public partial class Cell : BaseRigidBody3D, IPressable
{
    public void Spawn(Vector3 position)
    {
        GlobalPosition = position;

        Vector3 upwardImpulse = new Vector3(
            (float)GD.RandRange(-0.5, 0.5),
            (float)GD.RandRange(3.0, 5.0),
            (float)GD.RandRange(-0.5, 0.5)
        );

        ApplyImpulse(upwardImpulse);
    }

    public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        var pickParticle = Scenes.Partials.CellPick();
        this.FindParentOfType<World>().AddChild(pickParticle);
        pickParticle.GlobalPosition = GlobalPosition;
        playerController.Mutagen++;
        QueueFree();
    }

    public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }

    public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
    {
        
    }
}
