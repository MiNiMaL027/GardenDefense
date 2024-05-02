using Expand;
using Godot;

public partial class Farm : World
{
    public Area3D FarmArea { get; set; }
    public Funnel Funnel { get; set; }
    public Sickle Sickle { get; set; }
    public MobilePlanforms MobilePlanforms { get; set; }
    public PutArea PutArea { get; set; }
    public override void _Ready()
    {
        base._Ready();
        Funnel = GetNode<Funnel>("Funnel");
        Sickle = GetNode<Sickle>("Sickle");

        MobilePlanforms = GetNode<MobilePlanforms>("Enviroments/Components/MobilePlanforms");
        PutArea = GetNode<PutArea>("PutArea");

        FarmArea = GetNode<Area3D>("FarmArea");
        FarmArea.AreaEntered += FarmArea_AreaEntered;
        FarmArea.AreaExited += FarmArea_AreaExited;
    }
    private void FarmArea_AreaExited(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = false;
        }
    }

    private void FarmArea_AreaEntered(Area3D area)
    {
        if (area.Name == "CameraArea")
        {
            MusicCore.isFarm = true;
        }
    }
}
