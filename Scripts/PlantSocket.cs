using Enums;
using Godot;
using Interfaces;
using Items;
using System;
using System.Drawing;

public partial class PlantSocket : Area3D, IHoverable
{
    [Export]
    public SeedType SeedType;

    [Export]
    public int socketNumber;
    public bool IsUsed
    {
        get
        {
            return isUsed;
        }
        set
        {
            isUsed = value;
        }
    }
    private bool isUsed=false;
    private CollisionShape3D CollisionShape3D { get; set; }
    private MeshInstance3D MeshInstance3D { get; set; }
    public GrowingPlant GrowingPlant { get; set; }
    public override void _Ready()
    {
        CollisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
        MeshInstance3D = GetNode<MeshInstance3D>("MeshInstance3D");

        ChangeSize(SeedType);
    }

    private void ChangeSize(SeedType type)
    {
        Vector3 size;

        switch (type)
        {
            case SeedType.Small:
                size = new Vector3(0.2f, 0.2f, 0.2f);
                break;

            case SeedType.Big:
                size = new Vector3(0.3f, 0.3f, 0.3f);
                break;

            default:
                size = new Vector3(100,100, 100);
                break;
        }

        (CollisionShape3D.Shape as BoxShape3D).Size = size;
        (MeshInstance3D.Mesh as BoxMesh).Size = size;
    }

    public void MouseEnter()
    {
        ((MeshInstance3D.Mesh as BoxMesh).Material as StandardMaterial3D).EmissionEnergyMultiplier = 20;
    }

    public void MouseLeave()
    {
        ((MeshInstance3D.Mesh as BoxMesh).Material as StandardMaterial3D).EmissionEnergyMultiplier = 0;
    }
    public GrowingPlant Plant(int seedId)
    {
        GrowingPlant = Scenes.GrowingPlant();

        Pot parentPot = GetParent().GetParent<Pot>();

        parentPot.plantsContainer.AddChild(GrowingPlant);
        parentPot.DisableSockets();

        GrowingPlant.GlobalPosition = this.GlobalPosition;
        GrowingPlant.GlobalRotate(Vector3.Up, new Random().Next(0, 7));
        GrowingPlant.InfoSprite.GlobalRotation = Vector3.Zero;
        GrowingPlant.Init(seedId);
        GrowingPlant.SetWatered(parentPot.Watered);
        GrowingPlant.PlantSocket = this;


        IsUsed = true;
        return GrowingPlant;
    }
}
