using Expand;
using Godot;
using System;

public partial class World : Node3D
{
    public MusicCore MusicCore;
    public Area3D FarmArea { get; set; }
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();
    public Funnel Funnel { get; set; }
    public Sickle Sickle { get; set; }

    public MobilePlanforms MobilePlanforms { get; set; }
    public PutArea PutArea { get; set; }

    public override void _Ready()
    {
        Funnel = GetNode<Funnel>("Funnel");
        Sickle = GetNode<Sickle>("Sickle");

        MobilePlanforms = GetNode<MobilePlanforms>("Enviroments/Components/MobilePlanforms");
        PutArea = GetNode<PutArea>("PutArea");

        FarmArea = GetNode<Area3D>("FarmArea");

        MusicCore = GetNode<MusicCore>("MusicCore");

        FarmArea.AreaEntered += FarmArea_AreaEntered;
        FarmArea.AreaExited += FarmArea_AreaExited;
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
    private void FarmArea_AreaExited(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = false;
        }
    }

    private void FarmArea_AreaEntered(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = true;
        }
    }
}
