using Godot;
using Interfaces;
using Pawns;

namespace BaseClasses
{
    public partial class BaseOutlinePawn : Pawn, IHoverable
    {
        protected MeshInstance3D mesh;

        public override void _Ready()
        {
            mesh = GetChild<Node3D>(0).FindNthChild<MeshInstance3D>();

            base._Ready();
        }

        public virtual void MouseEnter()
        {
            ActiveOutline();
        }

        public virtual void MouseLeave()
        {
            UnactiveOutline();
        }

        public void ActiveOutline()
        {
            if (mesh.Mesh.SurfaceGetMaterial(0) == null)
                return;

            for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); i++)
            {
                var mat = mesh.Mesh.SurfaceGetMaterial(i).Duplicate() as StandardMaterial3D;
                mat.NextPass = ResourceLoader.Load<ShaderMaterial>("res://Shaders/Materials/Outline.tres");
                mesh.Mesh.SurfaceSetMaterial(i, mat);
            }
        }

        public void UnactiveOutline()
        {
            if (!IsInstanceValid(mesh))
                return;
            if (mesh.Mesh.SurfaceGetMaterial(0) == null)
                return;

            for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); i++)
            {
                mesh.Mesh.SurfaceGetMaterial(i).NextPass = null;
            }
        }
    }
}
