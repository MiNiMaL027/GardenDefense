using Godot;
using Interfaces;

namespace BaseClasses
{
    public partial class BaseRigidBody3D : RigidBody3D, IHoverable
    {
        protected MeshInstance3D mesh;

        public override void _Ready()
        {
            mesh = GetChild<Node3D>(0).FindNthChild<MeshInstance3D>();
        }

        public void MouseEnter()
        {
            if (IsInstanceValid(this)) 
            {
                ActiveOutline();
            }
        }

        public void MouseLeave()
        {
            if (IsInstanceValid(this))
            {
                UnactiveOutline();
            }
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
            if (mesh == null || mesh.IsQueuedForDeletion())
                return;

            if (mesh.Mesh == null || mesh.Mesh.SurfaceGetMaterial(0) == null)
                return;

            for (int i = 0; i < mesh.GetSurfaceOverrideMaterialCount(); i++)
            {
                mesh.Mesh.SurfaceGetMaterial(i).NextPass = null;
            }
        }
    }
}
