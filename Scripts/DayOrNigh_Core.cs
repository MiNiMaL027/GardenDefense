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

        UpdateRotationByDataTime();

        updateTimer = GetNode<Timer>("UpdateTimer");
        updateTimer.Timeout += UpdateTimer_Timeout;
    }

    private void UpdateTimer_Timeout()
    {
        UpdateRotationByDataTime();
    }

    private void UpdateRotationByDataTime()
    {
        DateTime currentTime = DateTime.Now;
        float currentHour = currentTime.Hour + currentTime.Minute / 60.0f;

        float rotationX = Mathf.Lerp(rotationXAtMidnight, rotationXAtNoon, currentHour / 12.0f);

        RotationDegrees = new Vector3(rotationX, 0.0f, 0.0f);
        GD.Print(rotationX);

        float ratio = (float)(currentTime.Hour * 60) / (12 * 60);

        float energyValue = 0.05f + (0.9f * ratio);

        GD.Print(env);

        (env.Environment.Sky.SkyMaterial as ProceduralSkyMaterial).SkyEnergyMultiplier = energyValue;
        (env.Environment.Sky.SkyMaterial as ProceduralSkyMaterial).GroundEnergyMultiplier = energyValue;

        GD.Print(energyValue);
    }
}
