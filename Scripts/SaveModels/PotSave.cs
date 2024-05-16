using BinarySerialization;
using System.Collections.Generic;

namespace SaveModels
{
    public class PotSave
    {
        [FieldOrder(0)]
        public int ItemId;

        [FieldOrder(1)]
        public int Transform3DLength { get; set; }

        [FieldOrder(2)]
        [FieldLength(nameof(Transform3DLength))]
        public TransformSave Transform3D;


        [FieldOrder(3)]
        public double WateredLeftTime;

        [FieldOrder(4)]
        public double FertilizedLeftTime;
        [FieldOrder(5)]
        public int AppliedFertilizerId;

        [FieldOrder(6)]
        public int GrowingPlantsArrayLength;
        [FieldOrder(7)]
        [FieldLength(nameof(GrowingPlantsArrayLength))]
        public List<GrowingPlantSave> GrowingPlants;

        public PotSave() { }
    }
}
