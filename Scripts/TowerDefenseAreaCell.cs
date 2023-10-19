using Godot;
using System;

public partial class TowerDefenseAreaCell : Area3D
{
	int cellX, cellY;
	MeshInstance3D meshInstance3D;
	public const int CellSizeX = 1;
    public const int CellSizeY = 1;
	static Material GreenMaterial;
    static Material YellowMaterial;
    static Material RedMaterial;

    static TowerDefenseAreaCell()
	{
		GreenMaterial= GD.Load<Material>("res://Meterials/Expand/Entered.tres");
        YellowMaterial = GD.Load<Material>("res://Meterials/Expand/Active.tres");
        RedMaterial = GD.Load<Material>("res://Meterials/Expand/Inactive.tres");

    }
    public override void _Ready()
	{
		meshInstance3D = GetNode<MeshInstance3D>("MeshInstance3D");
		meshInstance3D.Visible = false;
	}
    public override void _Notification(int what)
    {
        base._Notification(what);
		switch(what)
		{
			case Notifications.TowerDefenseAreaCell.HIGHLIGHT:
				meshInstance3D.Visible = true;
				break;
			case Notifications.TowerDefenseAreaCell.CANCEL_HIGHLIGHT:
                meshInstance3D.Visible = false;
				break;
        }
    }
    public void Init(int cellXToSet, int cellYToSet)
	{
		cellX= cellXToSet;
		cellY= cellYToSet;
	}
    public void MouseEnter()
    {
        if(CanPlant() == true) //there is alreadytwo children(meshInstance3d and collisionshape)
        {
            meshInstance3D.SetSurfaceOverrideMaterial(0, GreenMaterial);
        }
    }

    public void MouseLeave()
    {
        if (CanPlant() == true)
        {
            meshInstance3D.SetSurfaceOverrideMaterial(0, YellowMaterial);
        }
        else
        {
            meshInstance3D.SetSurfaceOverrideMaterial(0, RedMaterial);
        }
    }

    public bool CanPlant()
    {
        return GetChildCount() <= 2;//there is already two children(meshInstance3d and collisionshape)
    }
}
