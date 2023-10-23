using Godot;
using System;

public partial class MusicCore : Node
{
    AudioStreamPlayer Ambient;
    AudioStreamPlayer Drum;
    AudioStreamPlayer Farm_Part;
    AudioStreamPlayer Battle_Part;

    AnimationPlayer Animation;

    string musicPath = "res://Sounds/Musics/";
    string _currentMusicPath;
    string currentMusicPath
    {
        get
        {
            return _currentMusicPath;
        }
        set
        {
            Ambient.Stream = ResourceLoader.Load<AudioStreamOggVorbis>(value + "Ambient.ogg");
            Drum.Stream = ResourceLoader.Load<AudioStreamOggVorbis>(value + "Drum.ogg");
            Farm_Part.Stream = ResourceLoader.Load<AudioStreamOggVorbis>(value + "Farm.ogg");
            Battle_Part.Stream = ResourceLoader.Load<AudioStreamOggVorbis>(value + "Battle.ogg");

            _currentMusicPath = value;
        }
    }
    bool musicPlaying = false;
    bool ambientPlaying = false;

    bool _isFarm = true;
    public bool isFarm
    {
        get
        {
            return _isFarm;
        }
        set
        {
            if (value == _isFarm)
            {
                _isFarm = value;
                return;
            }

            if (value)
            {
                Animation.PlayBackwards("ChangeFarmOrBattle");

            }
            else
            {
                Animation.Play("ChangeFarmOrBattle");
            }

            _isFarm = value;
        }
    }

    public override void _Ready()
    {
        Ambient = GetNode<AudioStreamPlayer>("Ambient");
        Drum = GetNode<AudioStreamPlayer>("Drum");
        Farm_Part = GetNode<AudioStreamPlayer>("Farm_Part");
        Battle_Part = GetNode<AudioStreamPlayer>("Battle_Part");

        Animation = GetNode<AnimationPlayer>("AnimationPlayer");

        Ambient.Finished += Ambient_Finished;
        Farm_Part.Finished += Farm_Part_Finished;

        ChoseRandomAmbient();
        AmbientPlay();
        MusicPlay();
    }

    private void Farm_Part_Finished()
    {
        musicPlaying = false;
    }

    private void Ambient_Finished()
    {
        Random rnd = new Random();

        if (rnd.Next(0, 100) <= 30 && !musicPlaying)
        {
            MusicPlay();
        }
    }

    private void ChoseRandomAmbient()
    {
        Random rnd = new Random();
        var musicId = rnd.Next(1, 1);

        currentMusicPath = musicPath + $"{musicId}/";

        musicPlaying = false;
    }

    public void AmbientPlay()
    {
        Ambient.Play();
        Drum.Play();

        if (isFarm)
        {
            Drum.VolumeDb = -80;
        }
        else
        {
            Drum.VolumeDb = 0;
        }

        ambientPlaying = true;
    }

    public void AmbientStop()
    {
        Ambient.Stop();
        Drum.Stop();

        ambientPlaying = false;
    }

    public void MusicPlay()
    {
        Farm_Part.Play();
        Battle_Part.Play();

        musicPlaying = true;
    }
}
