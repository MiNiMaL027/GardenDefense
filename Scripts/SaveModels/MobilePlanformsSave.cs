using BinarySerialization;
using Godot;

namespace SaveModels
{
    public class MobilePlanformsSave
    {
        [FieldOrder(0)]
        public Vector3Save RightPlatformPosition;

        [FieldOrder(1)]
        public Vector3Save LowerPlatformPosition;

        [FieldOrder(2)]
        public ExpandActiveAreaSave RightAreaSave;

        [FieldOrder(3)]
        public ExpandActiveAreaSave LowerAreaSave;

        [FieldOrder(4)]
        public float StageToExpandRight_Value;

        [FieldOrder(5)]
        public float StageToExpandLower_Value;

        [FieldOrder(6)]
        public int CostToExpand;

        public MobilePlanformsSave() { }
    }
}
