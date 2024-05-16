using BinarySerialization;
using Godot;
using System.IO;

namespace SaveModels
{
    public class GameSave
    {
        const string SAVE_PATH = "user://gameSave.save";
        public const string ExactDateTimePattern = "yyyy-MM-dd HH:mm:ss.fffffff";


        [FieldOrder(0)]
        public int PlayerSaveLength { get; set; }

        [FieldOrder(1)]
        [FieldLength(nameof(PlayerSaveLength))]
        public PlayerSave PlayerSave;

        [FieldOrder(2)]
        public int FarmSaveLength { get; set; }

        [FieldOrder(3)]
        [FieldLength(nameof(FarmSaveLength))]
        public FarmSave FarmSave;

        

        public void SaveToFile()
        {
            MemoryStream stream = new MemoryStream();
            BinarySerializer serializer = new BinarySerializer();
            serializer.Serialize(stream, this);
            Godot.FileAccess file = Godot.FileAccess.Open(SAVE_PATH, Godot.FileAccess.ModeFlags.Write);
            file.StoreBuffer(stream.GetBuffer());
            file.Close();
        }
        public static GameSave LoadFromFile()
        {
            if (Godot.FileAccess.FileExists(SAVE_PATH) == false) { return null; }
            Godot.FileAccess file = Godot.FileAccess.Open(SAVE_PATH, Godot.FileAccess.ModeFlags.Read);
            byte[] bytes = file.GetBuffer((long)file.GetLength());
            file.Close();
            BinarySerializer serializer = new BinarySerializer();
            GameSave loadedSave = serializer.Deserialize<GameSave>(bytes);
            return loadedSave;
        }
        public static void DeleteSave()
        {
            if (Godot.FileAccess.FileExists(SAVE_PATH) == false) { return; }
            DirAccess.RemoveAbsolute(SAVE_PATH);
        }
        public GameSave() { }
    }
}
