using Godot;

public partial class World : Node3D
{
    public MusicCore MusicCore;
    [Export]
    public Vector2 MaxMapExtent = new Vector2();
    [Export]
    public Vector2 MinMapExtent = new Vector2();
    public override void _Ready()
    {
        MusicCore = GetNode<MusicCore>("MusicCore");   
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
    public void ClearWorld()
    {
        Godot.Collections.Array<Node> items = GetTree().GetNodesInGroup(Groups.Item);
        foreach (Node item in items)
        {
            item.QueueFree();
        }
    }
}
