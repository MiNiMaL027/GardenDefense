using Godot;
using System;

public partial class DayOrNigh_Core : Node3D
{
    [Export]
    public float rotationXAtMidnight = 90.0f; // Поворот о 00:00

    [Export]
    public float rotationXAtNoon = 270.0f;    // Поворот о 12:00

    public Timer updateTimer;

    public WorldEnvironment env;

    public override void _Ready()
    {
        env = GetParent<WorldEnvironment>();
        updateTimer = GetNode<Timer>("UpdateTimer");

        Options.DayOrNightChanged += init;

        if (!Options.nightOrDayCore)
            return;

        init(true);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        Options.DayOrNightChanged -= init;
    }

    private void init(bool enabled)
    {
        if (enabled)
        {           
            UpdateRotationByDataTime();
          
            updateTimer.Timeout += UpdateTimer_Timeout;
        }
        else
        {
            var currentDate = DateTime.Now;
            UpdateRotationByDataTime(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 12, 0, 0));
            updateTimer.Timeout -= UpdateTimer_Timeout;
        }      
    }

    private void UpdateTimer_Timeout()
    {
        UpdateRotationByDataTime();
    }

    private void UpdateRotationByDataTime(DateTime currentTime = default)
    {
        if(currentTime == default)
            currentTime = DateTime.Now;      
        
        float currentHour = currentTime.Hour + currentTime.Minute / 60.0f;

        float rotationX = Mathf.Lerp(rotationXAtMidnight, rotationXAtNoon, currentHour / 12.0f);
        RotationDegrees = new Vector3(rotationX, 0.0f, 0.0f);

        float ratio = 1.0f - (currentTime.Hour * 60 + currentTime.Minute) / 1440f;


        var proceduralSkyMaterial = env.Environment.Sky.SkyMaterial as ProceduralSkyMaterial;
        proceduralSkyMaterial.SkyEnergyMultiplier = Mathf.Lerp(0.05f, 1.0f, ratio);
        proceduralSkyMaterial.GroundEnergyMultiplier = Mathf.Lerp(0.05f, 1.0f, ratio);
    }
}
