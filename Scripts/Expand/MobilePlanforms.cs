using Godot;

namespace Expand
{
    public partial class MobilePlanforms : Node3D
    {
        StaticBody3D RightPlatform { get; set; }
        StaticBody3D LowerPlatform { get; set; }

        ExpandActiveArea RightArea { get; set; }
        ExpandActiveArea LowerArea { get; set; }

        public const float MaxRightExpand_Value = 20;

        public const float MaxLowerExpand_Value = 10;

        [Export(PropertyHint.Range, $"0,20,1")]
        public float StageToExpandRight_Value = 4;
        [Export(PropertyHint.Range, $"0,10,1")]
        public float StageToExpandLower_Value = 2;

        public int CostToExpand = 100;

        public override void _Ready()
        {
            RightPlatform = GetNode<StaticBody3D>("RightMobilePlatform");
            LowerPlatform = GetNode<StaticBody3D>("LowerMobilePlatform");


            RightArea = GetNode<ExpandActiveArea>("RightActiveArea");
            LowerArea = GetNode<ExpandActiveArea>("LowerActiveArea");
        }

        public void ToExpandLower()
        {
            var AdditionalValue = new Vector3(0, 0, StageToExpandLower_Value);

            LowerPlatform.Position += AdditionalValue;

            //(RightPlatform.GetChild<MeshInstance3D>(0).Mesh as BoxMesh).Size += AdditionalValue;
            //(RightPlatform.GetChild<CollisionShape3D>(1).Shape as BoxShape3D).Size += AdditionalValue;
            //RightPlatform.GlobalPosition += AdditionalValue/2;

            RightArea.Expand(0, StageToExpandLower_Value);
            LowerArea.Move(0, StageToExpandLower_Value);

            this.GetPlayerController().Gold -= CostToExpand;
            CostToExpand *= 2;

            ToShow();

            if (LowerPlatform.Position.Z == MaxLowerExpand_Value)
                LowerArea.QueueFree();
        }

        public void ToExpandRigth()
        {
            var AdditionalValue = new Vector3(StageToExpandRight_Value, 0, 0);

            RightPlatform.Position += AdditionalValue;

            //(LowerPlatform.GetChild<MeshInstance3D>(0).Mesh as BoxMesh).Size += AdditionalValue;
            //(LowerPlatform.GetChild<CollisionShape3D>(1).Shape as BoxShape3D).Size += AdditionalValue;
            //LowerPlatform.Position += AdditionalValue/2;

            LowerArea.Expand(StageToExpandRight_Value, 0);
            RightArea.Move(StageToExpandRight_Value, 0);

            this.GetPlayerController().Gold -= CostToExpand;
            CostToExpand *= 2;

            ToShow();

            if (RightPlatform.Position.X == MaxRightExpand_Value)
                RightArea.QueueFree();
        }

        public void ToShow()
        {
            var isEnoughtMoney = this.GetPlayerController().Gold >= CostToExpand;

            RightArea.ToShow(isEnoughtMoney, CostToExpand);
            LowerArea.ToShow(isEnoughtMoney, CostToExpand);
        }

        public void ToHide()
        {
            RightArea.ToHide();
            LowerArea.ToHide();
        }
    }
}

