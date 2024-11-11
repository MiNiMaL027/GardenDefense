using BinarySerialization;
using Godot;

namespace SaveModels
{
    public class ExpandActiveAreaSave
    {
        [FieldOrder(0)]
        public Vector3Save Position;

        [FieldOrder(1)]
        public Vector3Save MeshSize;

        [FieldOrder(2)]
        public Vector3Save CollisionShapeSize;

        [FieldOrder(3)]
        public Vector3Save LabelPosition;

        [FieldOrder(4)]
        public Vector3Save MeshPosition;

        [FieldOrder(5)]
        public bool IsActive;

        [FieldOrder(6)]
        public bool IsVisible;

        [FieldOrder(7)]
        public uint CollisionLayer;

        [FieldOrder(8)]
        public uint CollisionMask;

        [FieldOrder(9)]
        public int LabelTextLength;

        [FieldOrder(10)]
        [FieldLength(nameof(LabelTextLength))]
        public string LabelText;

        public ExpandActiveAreaSave() { }
    }
}
