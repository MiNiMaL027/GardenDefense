using Godot;
using System;

public partial class TestScript : MeshInstance3D
{
    public override void _Ready()
    {
        GD.Print("Ready.GlobalTransform.Basis.Z = " + GlobalTransform.Basis.Z);
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        GD.Print("_Process.GlobalTransform.Basis.Z = " + GlobalTransform.Basis.Z);
        GD.Print("_Process.Transform.Basis.Z = " + GlobalTransform.Basis.Z);


        GlobalTranslate(Transform.Basis.Z *(float)delta);
    }
}
