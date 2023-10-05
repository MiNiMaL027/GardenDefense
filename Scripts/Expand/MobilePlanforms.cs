using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class MobilePlanforms : Node3D
{
	StaticBody3D RightPlatform { get; set; }
	StaticBody3D LowerPlatform { get; set; }
	StaticBody3D SubsidaryPlatform { get; set; }

	ExpandActiveArea RightArea { get; set; }
	ExpandActiveArea LowerArea { get; set; }

	public const float MaxRightExpand_Value = 20;

	public const float MaxLowerExpand_Value = 10;

	[Export(PropertyHint.Range, $"0,20,1")]
	public float StageToExpandRight_Value = 4;
	[Export(PropertyHint.Range, $"0,10,1")]
	public float StageToExpandLower_Value = 2;

    public override void _Ready()
	{
		RightPlatform = GetNode<StaticBody3D>("RightMobilePlatform");
		LowerPlatform = GetNode<StaticBody3D>("LowerMobilePlatform");
		SubsidaryPlatform = GetNode<StaticBody3D>("SubsidiaryMobilePlatform");

		RightArea = GetNode<ExpandActiveArea>("RightActiveArea");
		LowerArea = GetNode<ExpandActiveArea>("LowerActiveArea");
    }

	public void ToExpandLower()
	{
		if (LowerPlatform.Position.Z == MaxLowerExpand_Value)
			throw new Exception("Uncorect platform position");

		var AdditionalValue = new Vector3(0, 0, StageToExpandLower_Value);

        LowerPlatform.Position += AdditionalValue;
		SubsidaryPlatform.Position += AdditionalValue;

		(RightPlatform.GetChild<MeshInstance3D>(0).Mesh as BoxMesh).Size += AdditionalValue;
		(RightPlatform.GetChild<CollisionShape3D>(1).Shape as BoxShape3D).Size += AdditionalValue;
		RightPlatform.Position += AdditionalValue;

		RightArea.Expand(0, StageToExpandLower_Value);
    }

	public void ToExpandRigth()
	{
		if(RightPlatform.Position.X == MaxRightExpand_Value)
            throw new Exception("Uncorect platform position");

		var AdditionalValue = new Vector3(StageToExpandRight_Value, 0, 0);

        RightPlatform.Position += AdditionalValue;
		SubsidaryPlatform.Position += AdditionalValue;

		(LowerPlatform.GetChild<MeshInstance3D>(0).Mesh as BoxMesh).Size += AdditionalValue;
		(LowerPlatform.GetChild<CollisionShape3D>(1).Shape as BoxShape3D).Size += AdditionalValue;
		LowerPlatform.Position += AdditionalValue/2;

		LowerArea.Expand(StageToExpandRight_Value, 0);
    }

	public void ToShow(bool isEnoughtMoney)
	{
		RightArea.ToShow(isEnoughtMoney);
		LowerArea.ToShow(isEnoughtMoney);
	}

	public void ToHide()
	{
		RightArea.ToHide();
		LowerArea.ToHide();
	}
}
