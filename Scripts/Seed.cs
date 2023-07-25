using Godot;
using Items;

public partial class Seed : Sprite2D
{
    public SeedItem item { get; set; }
    public void Init(SeedItem item)
    {
        this.item = item;
        Texture = item.Icon;
        GD.Print("Init");
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = GetViewport().GetMousePosition();
        GD.Print("Mouse" + GetViewport().GetMousePosition());
        GD.Print("Mouse" + Position);
    }
}
