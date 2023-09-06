using Controllers;
using Farm.Scripts.Widgets.ToolTip;
using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// <summary>
    /// Looks for parent until parent is Hud and return Hud. If no Hud on parent path then return null
    /// </summary>
    /// <param name="c">Any control inside Hud</param>
    /// <returns>Parent Hud</returns>
    public static Hud GetHud(this Control c)
    {
        Node n = c.GetParent();
        while (n != null && n is not Hud) //not null and not hud
        {
            n = n.GetParent();
        }
        return n as Hud;
    }
    public static void InitVisual(this Node3D node, PackedScene meshSceneToLoad, List<Node> excluded = null)
    {
        ///remove all mesh related childs
        if(excluded != null)
        {
            Godot.Collections.Array<Node> children = node.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                if (!excluded.Contains(children[i]))
                    children[i].QueueFree();
            }
        }
        else
        {
            Godot.Collections.Array<Node> children = node.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                children[i].QueueFree();
            }
        }
        if (meshSceneToLoad == null) { return; }

        ///add mesh to scene
        Node3D meshToLoad = meshSceneToLoad.Instantiate<Node3D>();
        node.AddChild(meshToLoad);
        MigrateEverything(meshToLoad, node);
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

        MigrateEverything(meshToLoad, node);

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
    public static void MigrateEverything(Node target, Node newParent)
    {

        Godot.Collections.Array<Node> children = target.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            target.RemoveChild(children[i]);
            newParent.AddChild(children[i]);
        }
    }

    public static void MoveToInventory(this Item target, PlayerController controller)
    {
        controller.InventoryComponentSeeds.AddItem(target.Id, 1);      
        target.QueueFree();
    }
    public static void AdjustControlInViewport(this Control c, Vector2 desiredGlobalPosition)
    {
        c.Visible = false;
        c.Visible = true;
        Rect2 viewportRect = c.GetViewportRect();
      
        Rect2 controlRect = c.GetGlobalRect();
        float controlEndX = desiredGlobalPosition.X + controlRect.Size.X;
        float controlEndY = desiredGlobalPosition.Y + controlRect.Size.Y;

        if (controlEndX>viewportRect.Size.X)
        {
            desiredGlobalPosition.X -= controlRect.Size.X;
        }
        if(controlEndY>viewportRect.Size.Y)
        {
            desiredGlobalPosition.Y -= controlRect.Size.Y;
        }
        c.GlobalPosition = desiredGlobalPosition;
        if(c is BaseTooltip t)
        {
            t.PostInit();
        }
    }
}
