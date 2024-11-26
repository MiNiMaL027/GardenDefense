using BinarySerialization;
using Godot;
using System;
using System.IO;

namespace SaveModels
{
    public class SettingsSave
    {
        const string SAVE_PATH = "user://gameSettings.save";
        public void SaveToFile()
        {
            MemoryStream stream = new MemoryStream();
            BinarySerializer serializer = new BinarySerializer();
            serializer.Serialize(stream, this);
            Godot.FileAccess file = Godot.FileAccess.Open(SAVE_PATH, Godot.FileAccess.ModeFlags.Write);
            file.StoreBuffer(stream.GetBuffer());
            file.Close();
        }
        public static SettingsSave LoadFromFile()
        {
            if (Godot.FileAccess.FileExists(SAVE_PATH) == false) { return null; }
            Godot.FileAccess file = Godot.FileAccess.Open(SAVE_PATH, Godot.FileAccess.ModeFlags.Read);
            byte[] bytes = file.GetBuffer((long)file.GetLength());
            file.Close();
            BinarySerializer serializer = new BinarySerializer();
            SettingsSave loadedSave = serializer.Deserialize<SettingsSave>(bytes);
            return loadedSave;
        }
        public static void DeleteSave()
        {
            if (Godot.FileAccess.FileExists(SAVE_PATH) == false) { return; }
            DirAccess.RemoveAbsolute(SAVE_PATH);
        }

        public void ApplySettings()
        {
            MusicVolume = musicVolume;
            SoundVolume = soundVolume;
            NightOrDayCore = nightOrDayCore;
        }

        public SettingsSave()
        {
            musicVolume = 0;
            soundVolume = 0;
            InfoPanel = true;
            SafeSelling = true;
            nightOrDayCore = true;
        }
        
        private int musicVolume;
        [FieldOrder(0)]
        public int MusicVolume
        {
            get { return musicVolume; }
            set
            {
                if (value > 0)
                    value = 0;

                if (value <= -25)
                    value = -80;

                musicVolume = value;

                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), value);
            }
        }
        
        private int soundVolume;
        [FieldOrder(1)]
        public int SoundVolume
        {
            get { return soundVolume; }
            set
            {
                if (value > 0)
                    value = 0;

                if (value <= -25)
                    value = -80;

                soundVolume = value;

                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sound"), value);
            }
        }
        [FieldOrder(2)]
        public bool InfoPanel { get; set; }
        [FieldOrder(3)]
        public bool SafeSelling { get; set; }

        public event Action<bool> DayOrNightChanged;

        
        private bool nightOrDayCore;
        [FieldOrder(4)]
        public bool NightOrDayCore
        {
            get { return nightOrDayCore; }
            set
            {
                nightOrDayCore = value;
                DayOrNightChanged?.Invoke(value);
            }
        }
    }
}
