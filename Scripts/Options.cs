using Godot;

public static class Options
{
    private static int _musicVolume = 0;
    public static int musicVolume
    {
        get { return _musicVolume; }
        set
        {
            if (value > 0)
                value = 0;

            if (value <= -25)
                value = -80;

            _musicVolume = value;

            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), value);
        }
    }
    private static int _soundVolume = 0;
    public static int soundVolume
    {
        get { return _soundVolume; }
        set
        {
            if (value > 0)
                value = 0;

            if (value <= -25)
                value = -80;

            _soundVolume = value;

            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sound"), value);
        }
    }
    public static bool infoPanel { get; set; } = true;
    public static bool safeSelling { get; set; } = true;
}
