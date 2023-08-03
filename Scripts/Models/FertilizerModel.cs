using Farm.Scripts.Enums;

namespace Farm.Scripts.Models
{
    public class FertilizerModel
    {
        public FertilizerType FertilizerType { get; set;}
        public int NumberOfUses { get; set;}

        public FertilizerModel(Fertilizer fertilizer)
        {
            FertilizerType = fertilizer.FertilizerType;
            NumberOfUses = fertilizer.NumberOfUses;
        }
    }
}
