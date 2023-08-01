using Controllers;
using Godot;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class ExtensionMethods
{
    public static bool RemoveFromParent(this Node target)
    {
        Node parent = target.GetParent();
        if (parent != null)
        {
            parent.RemoveChild(target);
            return true;
        }
        return false;
    }
    public static GameInstance GetGameInstance(this Node gameNode)
    {
        return gameNode.GetTree().CurrentScene as GameInstance;
    }
    public static PlayerController GetPlayerController(this Node gameNode)
    {
        return gameNode.GetTree().GetFirstNodeInGroup(Groups.Player) as PlayerController;
    }

    public static void InitVisual(this Node3D node, PackedScene meshSceneToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = node.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is Node3D node3D)
            {
                node3D.QueueFree();
            }
        }
        if (meshSceneToLoad == null) { return; }

        ///add mesh to scene
        Node3D meshToLoad = meshSceneToLoad.Instantiate<Node3D>();
        node.AddChild(meshToLoad);


        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, node);
        meshToLoad.QueueFree();
    }

    public static void InitVisual(this Node3D node, Node3D meshToLoad)
    {
        ///remove all mesh related childs
        Godot.Collections.Array<Node> children = node.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is Node3D node3D)
            {
                node3D.QueueFree();
            }
            
        }
        if (meshToLoad == null) { return; }
        node.AddChild(meshToLoad);

        MigrateCollisionsAndMeshes(meshToLoad, meshToLoad.Scale, node);
        meshToLoad.QueueFree();
    }

    public static void MigrateCollisionsAndMeshes(Node target, Vector3 scale, Node newParent)
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
