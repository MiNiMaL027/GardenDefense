using BinarySerialization;
using Godot;

namespace SaveModels
{
    public class TransformSave
    {
        [FieldOrder(0)]
        public int TransformRow1Length;

        [FieldOrder(1)]
        [FieldLength(nameof(TransformRow1Length))]
        public float[] TranformRow1;

        [FieldOrder(2)]
        [FieldLength(nameof(TransformRow1Length))]
        public float[] TranformRow2;

        [FieldOrder(3)]
        [FieldLength(nameof(TransformRow1Length))]
        public float[] TranformRow3;

        public TransformSave() { }
        public TransformSave(Transform3D t)
        {
            TranformRow1 = new float[4];
            TranformRow1[0] = t[0, 0];
            TranformRow1[1] = t[1, 0];
            TranformRow1[2] = t[2, 0];
            TranformRow1[3] = t[3, 0];

            TranformRow2 = new float[4];
            TranformRow2[0] = t[0, 1];
            TranformRow2[1] = t[1, 1];
            TranformRow2[2] = t[2, 1];
            TranformRow2[3] = t[3, 1];

            TranformRow3 = new float[4];
            TranformRow3[0] = t[0, 2];
            TranformRow3[1] = t[1, 2];
            TranformRow3[2] = t[2, 2];
            TranformRow3[3] = t[3, 2];
        }
        public Transform3D GetTransform()
        {
            Vector3 column0 = new Vector3(TranformRow1[0], TranformRow2[0], TranformRow3[0]);
            Vector3 column1 = new Vector3(TranformRow1[1], TranformRow2[1], TranformRow3[1]);
            Vector3 column2 = new Vector3(TranformRow1[2], TranformRow2[2], TranformRow3[2]);
            Vector3 origin = new Vector3(TranformRow1[3], TranformRow2[3], TranformRow3[3]);
            return new Transform3D(column0, column1, column2, origin);


        }
    }
}
