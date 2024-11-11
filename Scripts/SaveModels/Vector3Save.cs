using BinarySerialization;
using Godot;
using System;


namespace SaveModels
{
    public class Vector3Save
    {
        [FieldOrder(0)]
        public float x;
        [FieldOrder(1)]
        public float y;
        [FieldOrder(2)]
        public float z;
        public Vector3Save() { }
        public Vector3Save(Vector3 v)
        {
            x=v.X; y=v.Y; z=v.Z;
        }
        public Vector3 GetVector3()
        {
            Vector3 v = new Vector3(x, y, z);
            return v;
        }
    }
}
