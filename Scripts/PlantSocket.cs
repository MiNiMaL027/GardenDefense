using Enums;
using Godot;
using System;
using System.Drawing;

public partial class PlantSocket : Area3D
{
    [Export]
    public SeedType SeedType;

    public bool isUsed { get; set; }

    private CollisionShape3D CollisionShape3D { get; set; }
    private MeshInstance3D MeshInstance3D { get; set; }


    public override void _Ready()
    {       
        GD.Print("Ready");
        CollisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
        MeshInstance3D = GetNode<MeshInstance3D>("MeshInstance3D");
        ChangeSize(SeedType);

        MouseEntered += Mouse_Entered;
        MouseExited += Mouse_Exited;
    }

    public void EnableVision()
    {
        if (isUsed)
            return;

        Visible = true;
    }

    public void DisableVisibility()
    {
        Visible = false;
    }

    private void ChangeSize(SeedType type)
    {
        Vector3 size;
        switch (type)
        {
            case SeedType.Small:
                size = new Vector3(0.2f, 0.2f, 0.2f);
                (CollisionShape3D.Shape as BoxShape3D).Size = size;
                (MeshInstance3D.Mesh as BoxMesh).Size = size;
                break;
            case SeedType.Big:
                size = new Vector3(0.4f, 0.4f, 0.4f);
                (CollisionShape3D.Shape as BoxShape3D).Size = size;
                (MeshInstance3D.Mesh as BoxMesh).Size = size;
                break;
        }       
    }

    private void Mouse_Entered()
    {
        ((MeshInstance3D.Mesh as BoxMesh).Material as StandardMaterial3D).EmissionEnergyMultiplier = 20;
        GD.Print("start");

    }

    private void Mouse_Exited()
    {
        ((MeshInstance3D.Mesh as BoxMesh).Material as StandardMaterial3D).EmissionEnergyMultiplier = 0;
    }
}
