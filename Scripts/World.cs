using Godot;
using System;

public partial class World : Node3D
{
    public AudioStreamPlayer3D PlayerAudio { get; set; }
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();
    public Funnel Funnel { get; set; }
    public Sickle Sickle { get; set; }

    public override void _Ready()
    {
        PlayerAudio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
        Funnel = GetNode<Funnel>("Funnel");
        Sickle = GetNode<Sickle>("Sickle");
    }

    public void ChangeBus(int number)
    {
        GD.Print(Funnel);
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
