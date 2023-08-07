using Godot;
using System;
using Widgets.GardenWidgets;
using ItemsId;
using Interfaces;

namespace Controllers
{
    public partial class PlayerController : Node3D
    {
        [Export]
        public float CameraSpeed = 10.0f;
        [Export]
        public float ZoomSpeed = 1;
        public Hud Hud { get; set; }
        public Camera3D Camera3D { get; set; }
        public bool isFrontView { get; set; }
        public AnimationPlayer CameraAnimation { get; set; }
        public Node3D CameraBase { get; set; }
        public IPressable CurrentPressedObject { get; set; }
        public IHoverable CurrentHoveredObject { get; set; }
        public Control OpenedContextMenu { get; set; }

        #region PlayerData
        public InventoryComponent InventoryComponentSeeds { get; set; }
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
            }
        }
        private int gold;
        #endregion

        #region CameraMovement
        public int cameraInputX = 0;
        public int cameraInputZ = 0;
        public float MaxZoomDistance = 8;
        public float MinZoomDistance = 3;
        public Vector3 MaxMapExtent;
        public Vector3 MinMapExtent;
        private Vector2 lastMousePos;
        private bool isRotating = false;
        const float rotationSpeed = 0.1f;

        #endregion
        public override void _Ready()
        {
            Hud = GetNode<Hud>("Hud");
            Camera3D = GetNode<Camera3D>("CameraBase/Camera3D");
            CameraBase = GetNode<Node3D>("CameraBase");
            CameraAnimation = GetNode<AnimationPlayer>("CameraBase/Camera3D/Animation");
            #region PlayerData init
            gold = 10;
            InventoryComponentSeeds = Scenes.InventoryComponent();
            InventoryComponentSeeds.AddItem(ItemId.Seeds.CarrotSeed, 10);
            InventoryComponentSeeds.AddItem(ItemId.Fertilizers.BigSpeedFertilizer, 10);
            InventoryComponentSeeds.AddItem(ItemId.Fertilizers.BigEnlargeFertilizer, 10);
            InventoryComponentSeeds.AddItem(ItemId.Fertilizers.BigReturningFertilizer, 10);
            #endregion
            Hud.DisplayGardenWidget(this);
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
            query.CollideWithAreas = true;
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
            if (e is InputEventMouseButton eventMouseButtonLeft && eventMouseButtonLeft.ButtonIndex == MouseButton.Left)
            {
                RemoveOpenedContextMenu();
                if (eventMouseButtonLeft.DoubleClick)
                {
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
                        if (resultBody is IPressable pressable && pressable is Item item)
                        {
                            item.MoveToInventory(this);
                        }
                    }
                }
                else if (eventMouseButtonLeft.Pressed)
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
                        }
                    }
                }
                else
                {
                    CurrentPressedObject?.LeftMouseUpListener(eventMouseButtonLeft, this);
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
                if (Hud.BestiaryWindow == null)
                {
                    Hud.OpenBestiary();
                }
                else
                {
                    Hud.CloseBestiary();
                }
            }
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
            }
            else if (!isIn && currentDistance < MaxZoomDistance)
            {
                Camera3D.Translate(Transform.Basis.Z * ZoomSpeed);
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
    }
}

