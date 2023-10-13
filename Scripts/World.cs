using Godot;
using System;

public partial class World : Node3D
{
    public AudioStreamPlayer PlayerMusicAudio { get; set; }
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();
    public Funnel Funnel { get; set; }
    public Sickle Sickle { get; set; }

    public MobilePlanforms MobilePlanforms { get; set; }

    public override void _Ready()
    {
        PlayerMusicAudio = GetNode<AudioStreamPlayer>("AudioStreamMusicPlayer");
        Funnel = GetNode<Funnel>("Funnel");
        Sickle = GetNode<Sickle>("Sickle");

        MobilePlanforms = GetNode<MobilePlanforms>("Enviroments/Components/MobilePlanforms");
    }

    public void AddEffect(bool change)
    {
        if (change)
        {
            AudioServer.AddBusEffect(AudioServer.GetBusIndex("Music"), GD.Load<AudioEffectLowPassFilter>("res://Sounds/Effects/new_audio_effect_low_pass_filter.tres"));
        }
        else
        {
            AudioServer.RemoveBusEffect(AudioServer.GetBusIndex("Music"), 0);
        }
            
    }
}
