using Godot;
using Items;
using System;

[Tool]
public partial class GrowingPlant : StaticBody3D
{
    public SeedDatabaseRow SeedData;
    public void Init(Seed seed)
    {
        SeedData = DbService.GetItem(seed.Id) as SeedDatabaseRow;
        string directoryPath = seed.MeshPath.Substring(0, seed.MeshPath.LastIndexOf('/'));
        //init with first stage 
        this.InitVisual(ResourceLoader.Load<PackedScene>(directoryPath + "/Stage1.tscn"));
    }
    
}
