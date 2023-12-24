using Controllers;
using Items;
using Godot;
using Interfaces;
using System;
using Pawns;
using Widgets.GardenWidgets;
using ItemsId;

public partial class TowerDefenseArea : Node3D
{
    #region GridSize
    [Export]
    public int GridWidth = 10;
    [Export]
    public int LastNorthernLine = -1;
    [Export]
    public int LastSouthernLine = 1;
    int lastPossibleSideId = 3;
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

        Vector2 nextCellPosition = Vector2.Zero;

        ///draw central line
        for(int i = 0; i < GridWidth; i++)
        {
            TowerDefenseAreaCell towerDefenseAreaCell = Scenes.TowerDefenseAreaCell();
            AddChild(towerDefenseAreaCell);
            towerDefenseAreaCell.Init(i, 0);
            towerDefenseAreaCell.Translate(new Vector3(nextCellPosition.X, 0, 0));
            nextCellPosition.X += TowerDefenseAreaCell.CellSizeX;
        }
        nextCellPosition.X = 0;

        ///draw northern lines
        nextCellPosition.Y = -TowerDefenseAreaCell.CellSizeY;
        for (int i = -1; i >= LastNorthernLine; i--)
        {
            for (int j = 0; j < GridWidth; j++)
            {
                TowerDefenseAreaCell towerDefenseAreaCell = Scenes.TowerDefenseAreaCell();
                AddChild(towerDefenseAreaCell);
                towerDefenseAreaCell.Init(j, i);
                towerDefenseAreaCell.Translate(new Vector3(nextCellPosition.X, 0, nextCellPosition.Y));
                nextCellPosition.X += TowerDefenseAreaCell.CellSizeX;
            }
            nextCellPosition.Y -= TowerDefenseAreaCell.CellSizeY;
            nextCellPosition.X = 0;
        }

        /////draw southern lines
        nextCellPosition.Y = TowerDefenseAreaCell.CellSizeX;
        for (int i = 1; i <= LastSouthernLine; i++)
        {
            for (int j = 0; j < GridWidth; j++)
            {
                TowerDefenseAreaCell towerDefenseAreaCell = Scenes.TowerDefenseAreaCell();
                AddChild(towerDefenseAreaCell);
                towerDefenseAreaCell.Init(j, i);
                towerDefenseAreaCell.Translate(new Vector3(nextCellPosition.X, 0, nextCellPosition.Y));
                nextCellPosition.X += TowerDefenseAreaCell.CellSizeX;
            }
            nextCellPosition.Y += TowerDefenseAreaCell.CellSizeY;
            nextCellPosition.X = 0;
        }

    }
    public void AddLine(OpenLineSide side)
    {
        if (side == OpenLineSide.North)
        {
            if (Math.Abs(LastNorthernLine) == lastPossibleSideId) return;
            AddLine(--LastNorthernLine);

        }
        else
        {
            if (LastSouthernLine == lastPossibleSideId) return;
            AddLine(++LastSouthernLine);
        }
    }
    public void AddLine(int lineId)
    {
        Vector2 nextCellPosition = new Vector2(0, lineId * TowerDefenseAreaCell.CellSizeY);
        for (int j = 0; j < GridWidth; j++)
        {
            TowerDefenseAreaCell towerDefenseAreaCell = Scenes.TowerDefenseAreaCell();
            AddChild(towerDefenseAreaCell);
            towerDefenseAreaCell.Init(j, lineId);
            towerDefenseAreaCell.Translate(new Vector3(nextCellPosition.X, 0, nextCellPosition.Y));
            nextCellPosition.X += TowerDefenseAreaCell.CellSizeX;
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

    public void SpawnMonster(int lineNumber, int monsterId)
    {
        PawnDatabaseRow pawnDatabaseRow = DbService.GetPawn(monsterId);
        AIController aIController = ResourceLoader.Load<PackedScene>(pawnDatabaseRow.DefaultAIScenePath).Instantiate<AIController>();
        GameInstance.World.AddChild(aIController);
        Vector3 lineStartGlobalPosition = this.GlobalPosition + new Vector3(0, 0, lineNumber * TowerDefenseAreaCell.CellSizeY);
        aIController.GlobalPosition = lineStartGlobalPosition + Vector3.Up * 2 + this.Basis.X * 12;
    }
}
