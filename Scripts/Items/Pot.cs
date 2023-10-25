using Controllers;
using Enums;
using Godot;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using Widgets.Global;
using Widgets.ToolTip;

namespace Items
{
    public partial class Pot : Item, IPressable, IHoverable
    {
        private OmniLight3D light;
        private Node3D socketsContainer;
        public Node3D plantsContainer;
        public Timer waterTimer;
        public Timer fertilizeTimer;
        public int SecondsTimeToDry = 300;
        public List<PlantSocket> sockets;
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
                ChangeVisualFertilizedOrNot(true);
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
            linearMovementModifier = 1;


        }
        /// <summary>
        /// Called after item initialization
        /// </summary>
        public override void PostInit()
        {
            socketsContainer = GetNode<Node3D>("Soсkets");
            plantsContainer = GetNode<Node3D>("Plants");

            mesh = GetChildren().OfType<MeshInstance3D>().FirstOrDefault();
            mesh.Mesh.ResourceLocalToScene = true;

            MainLayer = 3;
            MoveLayer = 1;

            if (wasInited == true)
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
            wasInited = true;

            ChangeVisualWateredOrNot(false);

            Audio = new AudioStreamPlayer3D();
            AddChild(Audio);

            base.PostInit();
        }

        private void FertilizeTimer_Timeout()
        {
            fertilizer = null;
            ChangeVisualFertilizedOrNot(false);
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
            if (GlobalPosition.Y >= 1)
                return;

            if (plantsContainer.GetChildCount() == 0 || plantsContainer.GetChildCount() > 0 && plantsContainer.GetChild<GrowingPlant>(0).SeedData.SeedType == type)
                for (int i = 0; i < sockets.Count; i++)
                {
                    if (sockets[i].SeedType == type && !sockets[i].IsUsed)
                    {
                        sockets[i].Visible = true;
                        sockets[i].CollisionLayer = 1;
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
            if (watered)
            {
                mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/WaterDirt_Material.tres").Duplicate() as BaseMaterial3D);
            }
            else
            {
                mesh.Mesh.SurfaceSetMaterial(1, ResourceLoader.Load<StandardMaterial3D>("res://Meterials/Dirt_Material.tres").Duplicate() as BaseMaterial3D);
            }

            ChangeVisualFertilizedOrNot(fertilizer != null);
        }

        private void ChangeVisualFertilizedOrNot(bool fertilized)
        {
            if (fertilized)
            {
                (mesh.Mesh.SurfaceGetMaterial(1) as StandardMaterial3D).EmissionEnabled = true;
                (mesh.Mesh.SurfaceGetMaterial(1) as StandardMaterial3D).EmissionTexture = ResourceLoader.Load<CompressedTexture2D>("res://Meterials/Fertilize.png");

            }
            else
            {
                (mesh.Mesh.SurfaceGetMaterial(1) as StandardMaterial3D).EmissionEnabled = false;
            }
        }

        public override void InitializeItem(Item i)
        {
            Pot itemToCopy = i as Pot;
            if (itemToCopy == null) { return; }
            editorItemId = itemToCopy.EditorItemId;
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
            editorItemId = i.Id;
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

        new private void MoveToMouse()
        {
            Vector2 mousePosition = GetViewport().GetMousePosition();

            Camera3D camera = GetViewport().GetCamera3D();
            Vector3 from = camera.ProjectRayOrigin(mousePosition);
            Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;

            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0 && (CollisionObject3D)result["collider"] != this)
            {
                Vector3 target = (Vector3)result["position"];
                this.LinearVelocity = linearMovementModifier * new Vector3(target.X - GlobalPosition.X, 0, target.Z - GlobalPosition.Z);
            }
        }

        public override void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            SetDeferred("global_rotation", Vector3.Zero);
            isDragging = true;
            LockRotation = true;

            PlayAudio(PickAudioPath);
        }

        public override void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            isDragging = false;
            LockRotation = false;

            if (!isSelected)
                mesh.Mesh.SurfaceGetMaterial(0).NextPass = null;

            MoveToMouse();

            PlayAudio(HandOffAudioPath);
        }

        public override void ShowTooltip()
        {
            tooltip = Scenes.Widgets.ToolTip.PotTooltip();
            PlayerController playerController = this.GetPlayerController();
            playerController.Hud.AddChild(tooltip);
            tooltip.ShowTooltip(this);
            playerController.Hud.AddAtMousePosition(tooltip);
        }

        public override void HideTooltip()
        {
            if (tooltip != null)
            {
                tooltip.HideTooltip();
                tooltip = null;
            }
        }

        public override void MoveToInventory(PlayerController controller)
        {
            if (plantsContainer.GetChildCount() > 0)
            {
                controller.Hud.GardenWidget.InfoWindow.AddInfoPanel("Pot is already used, wait to finish growing the plant and retry");
                return;
            }

            if (fertilizer != null)
            {
                WindowConfirmation w = this.GetPlayerController().Hud.DisplayWindowConfirmation("This Pot has fertilizer. If you move pot in inventory fertilizer will be lost, are you sure?");
                w.Confirm += () => base.MoveToInventory(controller);
            }
            else
            {
                base.MoveToInventory(controller);
            }
        }

        public override void ActiveOutline()
        {
            var mat = mesh.Mesh.SurfaceGetMaterial(0).Duplicate() as StandardMaterial3D;
            mat.NextPass = ResourceLoader.Load<ShaderMaterial>("res://Shaders/Materials/Outline.tres");
            mesh.Mesh.SurfaceSetMaterial(0, mat);
        }

        public override void UnactiveOutline()
        {
            mesh.Mesh.SurfaceGetMaterial(0).NextPass = null;
        }

        protected override void InitSounds()
        {
            DropAudioPath = "res://Sounds/Sounds/Items/HangOffPot.ogg";
            HandOffAudioPath = "res://Sounds/Sounds/Items/HangOffPot.ogg";
            HitOtherItemPath = "";
            PickAudioPath = "res://Sounds/Sounds/Items/PickPot.ogg";
        }

        protected override void Item_BodyEntered(Node body)
        {
            if(body is Pot)
            {
                PlayAudio("res://Sounds/Sounds/Items/PotHitPot.ogg");
            }
        }
    }
}
