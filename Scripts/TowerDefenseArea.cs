using Controllers;
using Farm.Scripts.Items;
using Godot;
using Interfaces;
using System;

[Tool]
public partial class TowerDefenseArea : Area3D, IHoverable
{
    private PlayerController PlayerController;
    #region Children
    private CollisionShape3D CollisionShape3D;
    private MeshInstance3D GreenCell;

    #endregion

    #region CellSize
    [Export]
	public Vector2 CellSize
	{
		get
		{
			return cellSize;
		}
		set
		{
            GD.Print("TowerDefenseArea.CellSize setter called");
            cellSize = value;
			GenerateArea();
		}
	}
    private Vector2 cellSize = Vector2.One;
    #endregion

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


    private void GenerateArea()
	{
		GD.Print("TowerDefenseArea.GenerateArea called");
        BoxShape3D newBoxShape = new BoxShape3D();
        newBoxShape.Size = new Vector3(gridSize.X * cellSize.X, 1, gridSize.Y * cellSize.Y);
        CollisionShape3D.SetDeferred("shape", newBoxShape);
    }
	public override void _Ready()
	{
        SetProcess(false);
        GD.Print("TowerDefenseArea.Ready called");
		CollisionShape3D = GetNode<CollisionShape3D>("CollisionShape3D");
        GreenCell = GetNode<MeshInstance3D>("GreenCell");
        GenerateArea();
	}
    public override void _Process(double delta)
    {
        base._Process(delta);
        ///line trace
        Vector2 mousePosition = GetViewport().GetMousePosition();

        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Camera3D camera = GetViewport().GetCamera3D();
        Vector3 from = camera.ProjectRayOrigin(mousePosition);
        Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.CollideWithAreas = true;
        var result = spaceState.IntersectRay(query);

        if (result.Count > 0)
        {
            Vector3 hitGlobalPosition = (Vector3)result["position"];
            Vector3 hitLocalPosition = hitGlobalPosition - this.GlobalPosition;
            GD.Print(hitLocalPosition);
        }
    }

    public void MouseEnter()
    {
        GD.Print("TowerDefenseArea.MouseEnter called");
        PlayerController = this.GetPlayerController();
        if(PlayerController.CurrentPressedObject is BattlePlantItem)
        {
            SetProcess(true);
        }
    }

    public void MouseLeave()
    {
        SetProcess(false);
    }
}
