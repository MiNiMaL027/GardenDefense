using BinarySerialization;

namespace SaveModels
{
    public class ItemSave
    {
        [FieldOrder(0)]
        public int ItemId;
        [FieldOrder(1)]
        public int Transform3DLength { get; set; }
        [FieldOrder(2)]
        [FieldLength(nameof(Transform3DLength))]
        public TransformSave Transform3D;
    }
}
