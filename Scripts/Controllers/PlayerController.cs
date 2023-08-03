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
        public AnimationPlayer CameraAnimation { get;set; }
        public Node3D CameraBase { get; set; }
        public InventoryComponent InventoryComponentSeeds { get; set; }
        public IPressable CurrentPressedObject { get; set; }
        public IHoverable CurrentHoveredObject { get; set; }
        public Control OpenedContextMenu { get; set; }

        #region CameraMovement
        public int cameraInputX = 0;
        public int cameraInputZ = 0;
        public float MaxZoomDistance = 100;
        public float MinZoomDistance = 5;
        public Vector2 MaxMapExtent;
        public Vector2 MinMapExtent;

        #endregion
        public override void _Ready()
        {
            Hud = GetNode<Hud>("Hud");
            Camera3D = GetNode<Camera3D>("CameraBase/Camera3D");
            CameraBase = GetNode<Node3D>("CameraBase");
            CameraAnimation = GetNode<AnimationPlayer>("CameraBase/Camera3D/Animation");
            InventoryComponentSeeds = Scenes.InventoryComponent();
            InventoryComponentSeeds.AddItem(ItemId.Seeds.CarrotSeed, 5);
            InventoryComponentSeeds.AddItem(4, 10);
            Hud.DisplayGardenWidget(this);
        }
        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            #region CameraMovement
            var newX = CameraBase.GlobalPosition.X + cameraInputX * CameraSpeed * (float)delta;
            var newZ = CameraBase.GlobalPosition.Z + cameraInputZ * CameraSpeed * (float)delta;
            newX = Mathf.Clamp(newX, MinMapExtent.X, MaxMapExtent.X);
            newZ = Mathf.Clamp(newZ, MinMapExtent.Y, MaxMapExtent.Y);
            CameraBase.GlobalPosition = new Vector3(newX, CameraBase.GlobalPosition.Y, newZ);
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
            query.CollideWithAreas= true;
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

            cameraInputX = Convert.ToInt32(Input.IsActionPressed("right")) - Convert.ToInt32(Input.IsActionPressed("left"));
            cameraInputZ = Convert.ToInt32(Input.IsActionPressed("down")) - Convert.ToInt32(Input.IsActionPressed("up"));
            if(cameraInputX != 0 || cameraInputZ != 0)
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

