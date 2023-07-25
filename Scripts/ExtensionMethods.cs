using Controllers;
using Godot;

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
}
