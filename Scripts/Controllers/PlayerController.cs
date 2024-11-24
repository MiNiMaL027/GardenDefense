using Enums;
using Godot;
using Interfaces;
using Items;
using Ids;
using System;
using System.Collections.Generic;
using Widgets.ContextMenu;
using Pawns;
using SaveModels;

namespace Controllers
{
    public partial class PlayerController : Node3D
    {
        [Signal]
        public delegate void EnergyUpdatedEventHandler(int newEnergy);
        [Export]
        public float CameraSpeed = 1.0f;
        [Export]
        public float ZoomSpeed = 0.1f;
        public Hud Hud { get; set; }
        public Camera3D Camera3D { get; set; }
        public bool isFrontView { get; set; }
        public AnimationPlayer CameraAnimation { get; set; }
        public Node3D CameraBase { get; set; }
        public IPressable CurrentPressedObject { get; set; }
        public IHoverable CurrentHoveredObject { get; set; }
        public ItemContextMenu OpenedContextMenu { get; set; }
        public Timer TimerPickupTimer { get; set; }
        public Timer TimerEnergyRestore { get; set; }


        public int BattlefieldEnergy
        {
            get
            {
                return battlefieldEnergy;
            }
            set
            {
                battlefieldEnergy = value;              
                EmitSignal(SignalName.EnergyUpdated, value);
            }
        }

        private float timeToEnergyRestoratio = 3f;
        public float TimeToEnergyRestoration {
            get { return timeToEnergyRestoratio; }
            set
            {
                TimerEnergyRestore.WaitTime = value;
                timeToEnergyRestoratio = value;
            }
        } 
        public int MaxEnergy { get; set; } = 10;

        private int battlefieldEnergy = 0;
        public void TimerEnergyRestore_Timeout()
        {
            if (this.BattlefieldEnergy < this.MaxEnergy)
            {
                this.BattlefieldEnergy++;
                if(this.BattlefieldEnergy > this.MaxEnergy)
                {
                    this.BattlefieldEnergy= this.MaxEnergy;
                }
            }          
        }

        #region PlayerData
        public InventoryComponent GardenInventory { get; set; }
        public InventoryComponent BattlefieldInventory { get; set; }
        public InventoryComponent CurrentInventory { get; set; }
        [Signal]
        public delegate void GoldChangeEventHandler(int gold);
        public int Gold
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
                Hud.GardenWidget.UpdateGold(value);
                EmitSignal(SignalName.GoldChange, value);
            }
        }
        private int gold;

        [Signal]
        public delegate void MutagenUpdateEventHandler(int mutagenCount);
        private int mutagen;
        public int Mutagen
        {
            get
            {
                return mutagen;
            }
            set
            {
                mutagen = value;
                //TODO HUD change
                EmitSignal(SignalName.MutagenUpdate, value);
            }
        }

        public Dictionary<ItemType, List<int>> bestiaryItems = new Dictionary<ItemType, List<int>>();
        public List<int> bestiaryMonsters = new List<int>();


        public Dictionary<ItemType, List<int>> avaliableShopItems = new Dictionary<ItemType, List<int>>();
        public List<int> avaliableBattlePlantId = new List<int>();

        public int currentLvl = 1;
        #endregion

        #region CameraMovement
        public int cameraInputX = 0;
        public int cameraInputZ = 0;
        public float MaxZoomDistance = 8;
        public float MinZoomDistance = 1.5f;
        public Vector3 MaxMapExtent;
        public Vector3 MinMapExtent;
        private Vector2 lastMousePos;
        private bool isRotating = false;
        const float rotationSpeed = 0.1f;

        #endregion

        public override void _Ready()
        {
            BattlefieldInventory = GetNode<InventoryComponent>("BattlefieldInventory");
            GardenInventory = GetNode<InventoryComponent>("GardenInventory");
            TimerEnergyRestore = GetNode<Timer>("TimerEnergyRestore");
            TimerEnergyRestore.WaitTime = TimeToEnergyRestoration;
            TimerEnergyRestore.Timeout += TimerEnergyRestore_Timeout;

            Hud = GetNode<Hud>("Hud");
            Camera3D = GetNode<Camera3D>("CameraBase/Camera3D");
            CameraBase = GetNode<Node3D>("CameraBase");
            CameraAnimation = GetNode<AnimationPlayer>("CameraBase/Camera3D/Animation");
            TimerPickupTimer = GetNode<Timer>("TimerPickupItem");
        }
        public void NewGameInit()
        {
            #region PlayerData init
            gold = 10;
            GardenInventory.AddItem(ItemId.Harvestable.Carrot, 10);
            GardenInventory.AddItem(ItemId.Harvestable.WhiteLily, 10);


            GardenInventory.AddItem(ItemId.Fertilizers.BigSpeedFertilizer, 10);
            GardenInventory.AddItem(ItemId.Fertilizers.BigEnlargeFertilizer, 10);
            GardenInventory.AddItem(ItemId.Fertilizers.BigReturningFertilizer, 10);
            GardenInventory.AddItem(ItemId.Pots.SmallPot, 10);
            GardenInventory.AddItem(ItemId.Pots.MiddlePot, 10);
            GardenInventory.AddItem(ItemId.Pots.BigPot, 10);
            GardenInventory.AddItem(ItemId.Seeds.CarrotSeed, 10);
            GardenInventory.AddItem(ItemId.Seeds.CornSeed, 10);
            GardenInventory.AddItem(ItemId.Seeds.PeaSeed, 10);
            GardenInventory.AddItem(ItemId.Seeds.WhiteLilySeed, 10);
            GardenInventory.AddItem(ItemId.BattlePlants.BattlePea, 10);
            GardenInventory.AddItem(ItemId.BattlePlants.BattleCorn, 10);
            GardenInventory.AddItem(ItemId.BattlePlants.BattleCarrot, 10);
            GardenInventory.AddItem(ItemId.BattlePlants.BattleWhiteLily, 10);



            AddNewItemToBestiariy(ItemId.Seeds.CarrotSeed);
            AddNewItemToBestiariy(ItemId.Seeds.CornSeed);
            AddNewItemToBestiariy(ItemId.Seeds.PeaSeed);
            AddNewItemToBestiariy(ItemId.Seeds.WhiteLilySeed);

            AddNewItemToBestiariy(ItemId.Fertilizers.BigEnlargeFertilizer);
            AddNewItemToBestiariy(ItemId.Fertilizers.BigSpeedFertilizer);
            AddNewItemToBestiariy(ItemId.Fertilizers.BigReturningFertilizer);
            AddNewItemToBestiariy(ItemId.Harvestable.Carrot);
            AddNewItemToBestiariy(ItemId.Harvestable.Corn);
            AddNewItemToBestiariy(ItemId.Harvestable.Pea);
            AddNewItemToBestiariy(ItemId.Harvestable.WhiteLily);

            AddNewItemToBestiariy(ItemId.Pots.SmallPot);
            AddNewItemToBestiariy(ItemId.Pots.MiddlePot);
            AddNewItemToBestiariy(ItemId.Pots.BigPot);
            AddNewItemToBestiariy(ItemId.BattlePlants.BattlePea);
            AddNewItemToBestiariy(ItemId.BattlePlants.BattleCorn);
            AddNewItemToBestiariy(ItemId.BattlePlants.BattleCarrot);
            AddNewItemToBestiariy(ItemId.BattlePlants.BattleWhiteLily);

            bestiaryMonsters.AddRange(new int[] { PawnId.Monsters.Ant, PawnId.Monsters.AntDog, PawnId.Monsters.Wasp });

            AddNewItemToLaboratory(ItemId.BattlePlants.BattlePea);
            AddNewItemToLaboratory(ItemId.BattlePlants.BattleCorn);
            AddNewItemToLaboratory(ItemId.BattlePlants.BattleCarrot);

            AddNewItemToShop(ItemId.Seeds.CarrotSeed);
            AddNewItemToShop(ItemId.Seeds.CornSeed);
            AddNewItemToShop(ItemId.Seeds.PeaSeed);
            AddNewItemToShop(ItemId.Fertilizers.BigEnlargeFertilizer);
            AddNewItemToShop(ItemId.Fertilizers.BigSpeedFertilizer);
            AddNewItemToShop(ItemId.Fertilizers.BigReturningFertilizer);
            AddNewItemToShop(ItemId.Harvestable.Carrot);
            AddNewItemToShop(ItemId.Harvestable.Corn);
            AddNewItemToShop(ItemId.Harvestable.Pea);
            AddNewItemToShop(ItemId.Pots.SmallPot);
            AddNewItemToShop(ItemId.Pots.MiddlePot);
            AddNewItemToShop(ItemId.Pots.BigPot);

            AddNewItemToShop(ItemId.BattlePlants.BattlePea);
            AddNewItemToShop(ItemId.BattlePlants.BattleCorn);
            AddNewItemToShop(ItemId.BattlePlants.BattleCarrot);
            #endregion
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            #region CameraMovement
            CameraBase.GlobalTranslate(CameraBase.GlobalTransform.Basis.X * cameraInputX * CameraSpeed * (float)delta);
            CameraBase.GlobalTranslate(CameraBase.GlobalTransform.Basis.Z * cameraInputZ * CameraSpeed * (float)delta);
            CameraBase.GlobalPosition = CameraBase.GlobalPosition.Clamp(MinMapExtent, MaxMapExtent);
            #endregion
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            ///line trace
            Vector2 mousePosition = GetViewport().GetMousePosition();

            PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
            Camera3D camera = GetViewport().GetCamera3D();
            Vector3 from = camera.ProjectRayOrigin(mousePosition);
            Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;

            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.CollideWithAreas =false;
            var result = spaceState.IntersectRay(query);

            if (result.Count > 0)
            {
                CollisionObject3D resultBody = result["collider"].AsGodotObject() as CollisionObject3D;
                if (resultBody is IHoverable hoverable) //detected hoverable
                {
                    if (hoverable == CurrentHoveredObject) { return; } //nothing to do if it is the same object
                    //if new object then call mouse leave on old and assign new currently hovered
                    CurrentHoveredObject?.MouseLeave();
                    hoverable.MouseEnter();

                    CurrentHoveredObject = hoverable;
                }
                else //detected not hoverable
                {
                    CurrentHoveredObject?.MouseLeave();

                    CurrentHoveredObject = null;
                }
            }
            else
            {
                CurrentHoveredObject?.MouseLeave();

                CurrentHoveredObject = null;
            }
        }
        public override void _UnhandledInput(InputEvent e)
        {
            base._UnhandledInput(e);
           
            if (e is InputEventMouseButton eventMouseButtonLeft && eventMouseButtonLeft.ButtonIndex == MouseButton.Left)
            {
                RemoveOpenedContextMenu();

                if (eventMouseButtonLeft.Pressed)
                {
                    ///line trace
                    Vector2 mousePosition = eventMouseButtonLeft.GlobalPosition;

                    PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
                    Camera3D camera = GetViewport().GetCamera3D();
                    Vector3 from = camera.ProjectRayOrigin(mousePosition);
                    Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
                    var query = PhysicsRayQueryParameters3D.Create(from, to);
                    var result = spaceState.IntersectRay(query);


                    if (result.Count > 0)
                    {
                        CollisionObject3D resultBody = result["collider"].AsGodotObject() as CollisionObject3D;

                        if (resultBody is IPressable pressable)
                        {
                            CurrentPressedObject = pressable;
                            CurrentPressedObject.LeftMouseDownListener(eventMouseButtonLeft, this);
                            if(pressable is Item)
                            {
                                TimerPickupTimer.Start();
                            }
                        }
                    }
                }
                else
                {
                    CurrentPressedObject?.LeftMouseUpListener(eventMouseButtonLeft, this);
                    if(CurrentPressedObject is Item item && TimerPickupTimer.TimeLeft != 0)
                    {
                        item.MoveToInventory(this);
                    }
                    CurrentPressedObject = null;
                }
            }
            else if (e is InputEventMouseButton eventMouseButtonRight && eventMouseButtonRight.ButtonIndex == MouseButton.Right)
            {
                
                if (eventMouseButtonRight.Pressed)
                {
                    RemoveOpenedContextMenu();
                    ///line trace
                    Vector2 mousePosition = eventMouseButtonRight.GlobalPosition;

                    PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
                    Camera3D camera = GetViewport().GetCamera3D();
                    Vector3 from = camera.ProjectRayOrigin(mousePosition);
                    Vector3 to = from + camera.ProjectRayNormal(mousePosition) * 1000;
                    var query = PhysicsRayQueryParameters3D.Create(from, to);
                    var result = spaceState.IntersectRay(query);


                    if (result.Count > 0)
                    {
                        CollisionObject3D resultBody = result["collider"].AsGodotObject() as CollisionObject3D;

                        if (resultBody is IPressable pressable)
                        {
                            pressable.RightMouseDownListener(eventMouseButtonRight, this);
                        }
                    }
                }
            }
            else if(e is InputEventMouseButton eventMouseButtonMiddle && eventMouseButtonMiddle.ButtonIndex == MouseButton.Middle)
            {
                if (eventMouseButtonMiddle.DoubleClick)
                {
                    ResetCameraRotation();
                }
                if (eventMouseButtonMiddle.Pressed)
                {
                    isRotating = true;
                    lastMousePos = eventMouseButtonMiddle.Position;
                }
                else
                {
                    isRotating = false;
                }
            }
            if (Input.IsActionJustPressed("OpenBestiary"))
            {
                Hud.OpenBestiary();
            }
            if (Input.IsActionJustPressed("CloseOrPause"))
            {
                Hud.Pause();
            }
            if (Input.IsActionJustPressed("OpenInventory"))
            {
                Hud.MainWidget.ToggleInventory();
            }

            #region CameraMovement

            if (isRotating == true && e is InputEventMouseMotion eventMouseMotion)
            {
                RotateCamera(eventMouseMotion.Relative);
                lastMousePos = eventMouseMotion.Position;
            }

            cameraInputX = Convert.ToInt32(Input.IsActionPressed("right")) - Convert.ToInt32(Input.IsActionPressed("left"));
            cameraInputZ = Convert.ToInt32(Input.IsActionPressed("down")) - Convert.ToInt32(Input.IsActionPressed("up"));


            if (cameraInputX != 0 || cameraInputZ != 0)
            {
                RemoveOpenedContextMenu();
            }
            if (Input.IsActionJustPressed("ZoomIn"))
            {
                RemoveOpenedContextMenu();
                ZoomCamera(true);
            }
            if (Input.IsActionJustPressed("ZoomOut"))
            {
                RemoveOpenedContextMenu();
                ZoomCamera(false);
            }

            if (Input.IsActionJustPressed("ChangeView"))
            {
                RemoveOpenedContextMenu();
                if (!isFrontView)
                {
                    EnableFrontView();
                }
                else
                {
                    DisableFrontView();
                }
            }

            #endregion
        }

        public void RemoveOpenedContextMenu()
        {
            if(OpenedContextMenu != null)
            {
                OpenedContextMenu.QueueFree();

                OpenedContextMenu = null;
            }
        }

        public void ZoomCamera(bool isIn)
        {
            float currentDistance = Camera3D.GlobalPosition.DistanceTo(CameraBase.GlobalPosition);

            if (isIn && currentDistance > MinZoomDistance)
            {
                Camera3D.Translate(-Transform.Basis.Z * ZoomSpeed);
                Camera3D.Rotate(Vector3.Right, Mathf.DegToRad(0.1f));
            }
            else if (!isIn && currentDistance < MaxZoomDistance)
            {
                Camera3D.Translate(Transform.Basis.Z * ZoomSpeed);
                Camera3D.Rotate(Vector3.Left, Mathf.DegToRad(0.1f));
            }
        }

        private void RotateCamera(Vector2 mouseDelta)
        {
            Transform3D transform = CameraBase.GlobalTransform; // Використовуємо базовий вузол камери
            transform.Origin = Vector3.Zero; // Встановлюємо початкову точку у нульову позицію
            transform = transform.Rotated(Vector3.Up, Mathf.DegToRad(-mouseDelta.X * rotationSpeed));
            transform.Origin = CameraBase.GlobalTransform.Origin; // Відновлюємо початкову позицію
            CameraBase.GlobalTransform = transform; // Застосову
        }

        private void ResetCameraRotation()
        {
            Transform3D transform = CameraBase.GlobalTransform;
            transform.Basis = Basis.Identity; // Обнуляємо матрицю обертання
            CameraBase.GlobalTransform = transform;
        }

        private void EnableFrontView()
        {
            CameraAnimation.Play("FrontView");           
            isFrontView = true;
        }

        private void DisableFrontView()
        {
            CameraAnimation.PlayBackwards("FrontView");
            isFrontView = false;
        }

        public void AddNewItemToBestiariy(int id)
        {
            var itemType = DbService.GetItemType(id);

            if (!bestiaryItems.ContainsKey(itemType))
            {
                bestiaryItems.Add(itemType, new List<int>() { id });
            }
            else
            {
                bestiaryItems[itemType].Add(id);
            }
        }

        public void AddNewItemToShop(int id)
        {
            var itemType = DbService.GetItemType(id);

            if (!avaliableShopItems.ContainsKey(itemType))
            {
                avaliableShopItems.Add(itemType, new List<int>() { id });
            }
            else
            {
                avaliableShopItems[itemType].Add(id);
            }
        }

        public void AddNewItemToLaboratory(int id)
        {
            var unlockWindow = Scenes.Widgets.UnlockWindow();
            Hud.AddChild(unlockWindow);
            unlockWindow.Init(id);

            avaliableBattlePlantId.Add(id);
        }
        public PlayerSave GetPlayerSave()
        {
            PlayerSave playerSave = new PlayerSave()
            {
                InventorySave = new InventorySave(GardenInventory),
                Gold = this.Gold,
                AvaliableBattlePlantId = avaliableBattlePlantId,
                BestiaryMonsters = bestiaryMonsters,
                BestiaryItemsSave = new AvailableItemsSave(bestiaryItems),
                AvaliableShopItemsSave = new AvailableItemsSave(avaliableShopItems),
                CurrentLvl = currentLvl
            };
            return playerSave;
        }

        internal void LoadFromSave(PlayerSave playerSave)
        {
            GardenInventory.LoadFromSave(playerSave.InventorySave);
            Gold=playerSave.Gold;
            avaliableBattlePlantId = playerSave.AvaliableBattlePlantId;
            bestiaryMonsters = playerSave.BestiaryMonsters;
            bestiaryItems = playerSave.BestiaryItemsSave.ToDictionary();
            avaliableShopItems=playerSave.AvaliableShopItemsSave.ToDictionary();
            currentLvl = playerSave.CurrentLvl;
        }
    }
}

