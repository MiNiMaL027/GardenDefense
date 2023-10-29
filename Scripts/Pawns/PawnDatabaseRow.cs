using Enums;

namespace Pawns
{
    public enum PawnDatabaseType
    {
        BattlePlant=0,
        Monster = 1,
    }
    public class PawnDatabaseRow
    {
        public int Id;
        public string Name;
        public string Description;
        public PawnDatabaseType PawnDatabaseType;
        public string TextureSpritePath;
        public string ScenePath;
        public string DefaultAIScenePath;
    }
}
