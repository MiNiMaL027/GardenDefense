using Controllers;
using Godot;
using Interfaces;
using SaveModels;
namespace Expand
{
    public partial class ExpandActiveArea : StaticBody3D, IPressable
    {
        public MeshInstance3D Instance { get; set; }
        public CollisionShape3D CollisionShape { get; set; }
        public Label3D Label { get; set; }

        public bool isActive = false;

        public override void _Ready()
        {
            Instance = GetNode<MeshInstance3D>("MeshInstance3D");
            CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
            Label = GetNode<Label3D>("Label3D");

            MouseEntered += ExpandActiveArea_MouseEntered;
            MouseExited += ExpandActiveArea_MouseExited;
        }

        private void ExpandActiveArea_MouseExited()
        {
            if (isActive)
            {
                Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/Active.tres"));
            }
        }

        private void ExpandActiveArea_MouseEntered()
        {
            if (isActive)
            {
                Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/Entered.tres"));
            }
        }

        public void Active()
        {
            isActive = true;
            Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/Active.tres"));
        }

        public void Inactive()
        {
            isActive = false;
            Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/inactive.tres"));
        }

        public void ToShow(bool isEnoughtMoney, int cost)
        {
            Visible = true;
            CollisionLayer = 1;
            CollisionMask = 1;

            if (isEnoughtMoney)
            {
                Active();
            }
            else
            {
                Inactive();
            }

            Label.Text = cost.ToString();
            GD.Print($"ExpandActiveArea {Name} = " + this.GlobalPosition);
        }

        public void ToHide()
        {
            Visible = false;
            CollisionLayer = 1;
            CollisionMask = 1;
        }

        public void Expand(float X, float Z)
        {
            (Instance.Mesh as BoxMesh).Size += new Vector3(X, 0, Z);
            (CollisionShape.Shape as BoxShape3D).Size += new Vector3(X, 0, Z);

            if (X == 0)
            {
                Instance.Position += new Vector3(0, 0, Z / 2);
                CollisionShape.Position += new Vector3(0, 0, Z / 2);
                Label.Position += new Vector3(0, 0, Z / 2);
            }
            else if (Z == 0)
            {
                Instance.Position += new Vector3(X / 2, 0, 0);
                CollisionShape.Position += new Vector3(X / 2, 0, 0);
                Label.Position += new Vector3(X / 2, 0, 0);
            }
        }

        public void Move(float X, float Z)
        {
            GlobalPosition += new Vector3(X, 0, Z);
        }

        public void LeftMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
            if (!isActive)
                return;

            if (Name == "RightActiveArea")
            {
                GetParent<MobilePlanforms>().ToExpandRigth();
            }
            else if (Name == "LowerActiveArea")
            {
                GetParent<MobilePlanforms>().ToExpandLower();
            }
        }

        public void RightMouseDownListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {

        }

        public void LeftMouseUpListener(InputEventMouseButton eventMouseButton, PlayerController playerController)
        {
        }

        public ExpandActiveAreaSave GetSave()
        {
            ExpandActiveAreaSave save = new ExpandActiveAreaSave
            {
                Position = new Vector3Save(GlobalPosition),
                MeshPosition = new Vector3Save(Instance.Position),
                MeshSize = new Vector3Save((Instance.Mesh as BoxMesh).Size),
                CollisionShapeSize = new Vector3Save((CollisionShape.Shape as BoxShape3D).Size),
                LabelPosition = new Vector3Save(Label.Position),
                IsActive = isActive,
                IsVisible = Visible,
                CollisionLayer = CollisionLayer,
                CollisionMask = CollisionMask,
                LabelTextLength = Label.Text.Length,
                LabelText = Label.Text
            };

            return save;
        }

        public void LoadSave(ExpandActiveAreaSave save)
        {
            GlobalPosition = save.Position.GetVector3();
            Instance.Position = save.MeshPosition.GetVector3();
            (Instance.Mesh as BoxMesh).Size = save.MeshSize.GetVector3();
            (CollisionShape.Shape as BoxShape3D).Size = save.CollisionShapeSize.GetVector3();
            Label.Position = save.LabelPosition.GetVector3();
            isActive = save.IsActive;
            Visible = save.IsVisible;
            CollisionLayer = save.CollisionLayer;
            CollisionMask = save.CollisionMask;
            Label.Text = save.LabelText;
        }
    }
}

