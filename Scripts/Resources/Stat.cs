using Godot;

[GlobalClass]
public partial class Stat : Resource
{
    [Export]
    public string StatName;
    [Export]
    public int StatValue;
}

