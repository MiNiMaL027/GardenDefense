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
        InitVisual(ResourceLoader.Load<PackedScene>(directoryPath + "/Stage1.tscn"));
    }
    public void InitVisual(PackedScene meshSceneToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = this.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            Node n = children[i] as Node;
            n.QueueFree();
        }
        if (meshSceneToLoad == null) { return; }

        ///add mesh to scene
        Node3D meshToLoad = meshSceneToLoad.Instantiate<Node3D>();
        AddChild(meshToLoad);


        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, this);
        meshToLoad.QueueFree();
    }
    public void InitVisual(Node3D meshToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = this.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            Node n = children[i];
            n.QueueFree();
        }
        if (meshToLoad == null) { return; }
        AddChild(meshToLoad);

        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, this);
        meshToLoad.QueueFree();
    }
    private void MigrateCollisionsAndMeshes(Node target, Vector3 scale, Node newParent)
    {
        Godot.Collections.Array<Node> children = target.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is CollisionShape3D collisionShape)
            {
                collisionShape.RemoveFromParent();
                newParent.AddChild(collisionShape);
                collisionShape.Scale *= scale;
            }
            else if (children[i] is MeshInstance3D meshInstance)
            {
                meshInstance.RemoveFromParent();
                newParent.AddChild(meshInstance);
                meshInstance.Scale *= scale;
            }
            else
            {
                if (children[i] is Node3D spatial)
                {
                    MigrateCollisionsAndMeshes(children[i], scale * spatial.Scale, newParent);

                }
                else
                {
                    MigrateCollisionsAndMeshes(children[i], scale, newParent);
                }
            }
        }
    }
}
