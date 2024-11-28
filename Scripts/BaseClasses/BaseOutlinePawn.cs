using Godot;
using Interfaces;
using Pawns;

namespace BaseClasses
{
    public partial class BaseOutlinePawn : Pawn, IHoverable
    {
        bool isInner;
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
                var originalMaterial = mesh.Mesh.SurfaceGetMaterial(i);

                ShaderMaterial outlineMaterial = ResourceLoader.Load<ShaderMaterial>("res://Shaders/Materials/Outline.tres");

                if (originalMaterial.NextPass != null)
                {
                    var currentNextPass = originalMaterial.NextPass.Duplicate() as ShaderMaterial;
                    currentNextPass.NextPass = outlineMaterial;
                    originalMaterial.NextPass = currentNextPass;
                    isInner = true;
                }
                else
                {
                    var duplicatedMaterial = originalMaterial.Duplicate() as StandardMaterial3D;
                    duplicatedMaterial.NextPass = outlineMaterial;
                    mesh.Mesh.SurfaceSetMaterial(i, duplicatedMaterial);
                    isInner = false;
                }
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
                var material = mesh.Mesh.SurfaceGetMaterial(i);

                if (isInner)
                {
                    material.NextPass.NextPass = null;
                }
                else
                {
                    material.NextPass = null;
                }
            }
        }
    }
}

