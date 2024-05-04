using BinarySerialization;
using System;
using System.Collections.Generic;

namespace SaveModels
{
    public class GrowingPlantSave
    {
        [FieldOrder(0)]
        public int PlantSocketNumber;

        [FieldOrder(1)]
        public int SeedId;
        [FieldOrder(2)]
        public int CurrentStage;
        [FieldOrder(3)]
        public string DateTimeStageBegin;
        [FieldOrder(4)]
        public int CropModifier;
        [FieldOrder(5)]
        public int numberOfSeedReturns;
        [FieldOrder(6)]
        public int availableCrop;

        public GrowingPlantSave(GrowingPlant growingPlant)
        {
            PlantSocketNumber = growingPlant.PlantSocket.socketNumber;
            SeedId = growingPlant.SeedData.Id;
            CurrentStage = growingPlant.CurrentStage;
            DateTimeStageBegin = growingPlant.dateTimeStageBegin.ToString(GameSave.ExactDateTimePattern);
            CropModifier = growingPlant.cropModifier;
            numberOfSeedReturns = growingPlant.numberOfSeedReturns;
            availableCrop = growingPlant.availableCrop;

        }
    }
}
