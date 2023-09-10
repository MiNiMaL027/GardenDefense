using Godot;
using System;

public partial class World : Node3D
{
    public AudioStreamPlayer3D PlayerAudio { get; set; }
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();

    public override void _Ready()
    {
        PlayerAudio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
    }

    public void ChangeBus(int number)
    {
        switch (number)
        {
            case 0:
                PlayerAudio.Bus = "Master";
                break;

            case 1:
                PlayerAudio.Bus = "Dully";
                break;
        }      
    }
}
