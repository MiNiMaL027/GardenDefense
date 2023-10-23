using Controllers;
using Items;
using Godot;
using Interfaces;
using System;

public partial class TowerDefenseArea : Node3D
{
    #region GridSize
    [Export]
    public Vector2 GridSize
    {
        get
        {
            return gridSize;
        }
        set
        {
            GD.Print("TowerDefenseArea.GridSize setter called");
            gridSize = value;
            GenerateArea();
        }
    }
    private Vector2 gridSize = new Vector2(10,10);
    #endregion

    public override void _Ready()
    {
        base._Ready();
        SetProcess(false);
        GenerateArea();
    }
    private void GenerateArea()
	{
        Godot.Collections.Array<Node> children = this.GetChildren();
        foreach (Node node in children)
        {
            node.QueueFree();
        }
        GD.Print("TowerDefenseArea.GenerateArea called");
        Vector2 nextCellPosition= Vector2.Zero;
        for(int i = 0; i < gridSize.X; i++)
        {
            for(int y = 0;y< gridSize.Y;y++)
            {
                TowerDefenseAreaCell towerDefenseAreaCell = Scenes.TowerDefenseAreaCell();
                AddChild(towerDefenseAreaCell);
                towerDefenseAreaCell.Init(i, y);
                towerDefenseAreaCell.Translate(new Vector3(nextCellPosition.X, 0, nextCellPosition.Y));
                nextCellPosition.Y += TowerDefenseAreaCell.CellSizeY;
            }
            nextCellPosition.X += TowerDefenseAreaCell.CellSizeX;
            nextCellPosition.Y = 0;
        }
    }
    public override void _Notification(int what)
    {
        base._Notification(what);
        switch(what)
        {
            case Notifications.TowerDefenseArea.ITEM_BATTLEPLANT_CAPTURED:
                GetTree().NotifyGroup(Groups.TowerDefenseAreaCell, Notifications.TowerDefenseAreaCell.HIGHLIGHT);
                SetProcess(true);
                break;
            case Notifications.TowerDefenseArea.ITEM_BATTLEPLANT_RELEASED:
                GetTree().NotifyGroup(Groups.TowerDefenseAreaCell, Notifications.TowerDefenseAreaCell.CANCEL_HIGHLIGHT);
                SetProcess(false);
                currentlyHoveredCell?.MouseLeave();
                currentlyHoveredCell = null;
                break;
        }
    }
    TowerDefenseAreaCell currentlyHoveredCell;
    public override void _Process(double delta)
    {
        base._Process(delta);
        ///line trace
        Vector2 mousePosition = GetViewport().GetMousePosition();

        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
        var query = PhysicsRayQueryParameters3D.Create(from, to, 1);
        query.CollideWithAreas = true;
        query.CollideWithBodies = false;
        var result = spaceState.IntersectRay(query);
        if (result.Count > 0)
        {
            CollisionObject3D resultBody = result["collider"].AsGodotObject() as CollisionObject3D;
            if (resultBody is TowerDefenseAreaCell cell) //detected cell
            {
                if (cell == currentlyHoveredCell) { return; } //nothing to do if it is the same object
                                                              //if new object then call mouse leave on old and assign new currently hovered
                currentlyHoveredCell?.MouseLeave();
                cell.MouseEnter();

                currentlyHoveredCell = cell;
            }
            else //detected not hoverable
            {
                currentlyHoveredCell?.MouseLeave();
                currentlyHoveredCell = null;
            }
        }
        else
        {
            currentlyHoveredCell?.MouseLeave();
            currentlyHoveredCell = null;

        }
    }
}
