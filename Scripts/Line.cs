using Godot;
using System.Collections.Generic;
public partial class Line : RayCast3D
{
    public static List<RayCast3D> Lines { get; set; } = new List<RayCast3D>();
    public static RayCast3D CreateLine(Vector3 from, Vector3 to, Color c, int Thickness)
    {
        GD.Print(from, to);
        RayCast3D r = new RayCast3D();
        Lines.Add(r);
        GameInstance.World.AddChild(r);
        r.CollisionMask = 2;
        r.GlobalPosition = from;
        r.TargetPosition = to - from;
        r.DebugShapeCustomColor = c;
        r.DebugShapeThickness = Thickness;
        r.Enabled = true;
        return r;
    }
    public static void RemoveLines()
    {
        foreach (var l in Lines)
        {
            l.QueueFree();
        }
        Lines.Clear();
    }
}
