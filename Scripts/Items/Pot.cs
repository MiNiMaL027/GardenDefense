using Enums;
using Godot;
using Items;
using System.Collections.Generic;
using System.Linq;

public partial class Pot : Item
{
    private OmniLight3D light;
    private Node3D socketsContainer;
    public Node3D plantsContainer;
    public Timer waterTimer;
    public Timer fertilizeTimer;
    public int SecondsTimeToDry = 300;
    public List<PlantSocket> sockets;
    public MeshInstance3D mesh;
    public PotTooltip tooltip;
    bool wasInited = false;

    private FertilizerDatabaseRow fertilizer;
    public FertilizerDatabaseRow Fertilizer
    {
        get { return fertilizer; }

        set 
        {
            //can't add another fertilizer and can't assign no furtilizer
            if (value == null || fertilizer != null) { return; }
            fertilizer = value;
            fertilizeTimer.WaitTime = fertilizer.SecondsDuration;
            fertilizeTimer.Start();
        }
    }
    private bool watered;
    public bool Watered
    {
        get { return watered; }
        set
        {
            waterTimer.Start(); 
            watered = value;

            ChangeVisualWateredOrNot(value);

            Godot.Collections.Array<Node> plantsGdArray = plantsContainer.GetChildren();

            for (int i = 0; i < plantsGdArray.Count; i++)
            {
                (plantsGdArray[i] as GrowingPlant).SetWatered(value);
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(Groups.Pot, true);
    }
    /// <summary>
    /// Called after item initialization
    /// </summary>
    private void PostInit()
    {
        light = GetNode<OmniLight3D>("Light");
        socketsContainer = GetNode<Node3D>("Soсkets");
        plantsContainer = GetNode<Node3D>("Plants");
        mesh = GetChildren().OfType<MeshInstance3D>().FirstOrDefault();

        if(wasInited == true)
        {
            waterTimer.Stop();
            waterTimer.QueueFree();

            fertilizeTimer.Stop();
            fertilizeTimer.QueueFree();

            sockets.Clear();
        }

        #region waterTimer
        waterTimer = new Timer();
        waterTimer.Autostart = false;
        waterTimer.WaitTime = SecondsTimeToDry;
        waterTimer.OneShot = true;
        AddChild(waterTimer);
        waterTimer.Timeout += WaterTimer_Timeout;
        #endregion

        #region fertilizeTimer
        fertilizeTimer = new Timer();
        fertilizeTimer.Autostart = false;
        fertilizeTimer.OneShot = true;
        AddChild(fertilizeTimer);
        fertilizeTimer.Timeout += FertilizeTimer_Timeout;
        #endregion

        ReadSockets();
        wasInited= true;
    }

    private void FertilizeTimer_Timeout()
    {
        fertilizer = null;
    }

    private void WaterTimer_Timeout()
    {
        Watered = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isDragging)
        {
            MoveToMouse();
        }
    }

    private void ReadSockets()
    {
        Godot.Collections.Array<Node> socketsGdArray = socketsContainer.GetChildren();
        sockets = new List<PlantSocket>(socketsGdArray.Count);
        for (int i = 0; i < socketsGdArray.Count; i++)
        {
            sockets.Add(socketsGdArray[i] as PlantSocket);
        }
    }

    public void EnableSockets(SeedType type)
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            if (sockets[i].SeedType == type && !sockets[i].IsUsed)
            {
                sockets[i].Visible = true;
                sockets[i].CollisionLayer= 1;
                sockets[i].CollisionMask = 1;
            }
        }
    }

    public void DisableSockets()
    {
        for (int i = 0; i < sockets.Count; i++)
        {
            sockets[i].Visible = false;
            sockets[i].CollisionLayer = 0;
            sockets[i].CollisionMask = 0;
        }
    }

    private void ChangeVisualWateredOrNot(bool watered)
    {
        if(watered)
        {
            mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/WaterDirt_Material.tres"));
        }
        else
        {
            mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/Dirt_Material.tres"));
        }
    }

    public override void InitializeItem(Item i)
    {
        Pot itemToCopy = i as Pot;
        if (itemToCopy == null) { return; }
        id = itemToCopy.Id;
        ItemName = itemToCopy.ItemName;
        BuyPrice = itemToCopy.BuyPrice;
        SellPrice = itemToCopy.SellPrice;
        Description = itemToCopy.Description;
        ItemType = itemToCopy.ItemType;
        MeshPath = itemToCopy.MeshPath;
        TextureSpritePath = itemToCopy.TextureSpritePath;
        SecondsTimeToDry = itemToCopy.SecondsTimeToDry;
        this.InitVisual(itemToCopy);
        PostInit();
    }
    public override void InitializeItem(int itemId)
    {
        PotDatabaseRow i = DbService.GetItem(itemId) as PotDatabaseRow;
        InitializeItem(i);
    }
    public override void InitializeItem(ItemDatabaseRow dbRow)
    {
        PotDatabaseRow i = dbRow as PotDatabaseRow;
        if (i.Id == 0) { return; } //not found
        id = i.Id;
        ItemName = i.ItemName;
        Description = i.Description;
        BuyPrice = i.BuyPrice;
        SellPrice = i.SellPrice;
        ItemType = i.ItemType;
        MeshPath = i.MeshPath;
        TextureSpritePath = i.TextureSpritePath;
        SecondsTimeToDry = i.WaterTime;
        PackedScene meshScene = ResourceLoader.Load<PackedScene>(MeshPath);
        this.InitVisual(meshScene);
        PostInit();

    }
}