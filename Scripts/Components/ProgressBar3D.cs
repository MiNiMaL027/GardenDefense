using Godot;
using System;
namespace Components
{
    public partial class ProgressBar3D : Sprite3D
    {
        ProgressBar ProgressBar { get; set; }
        public override void _Ready()
        {
            this.Texture = GetNode<SubViewport>("Viewport").GetTexture();
            ProgressBar = GetNode<ProgressBar>("Viewport/ProgressBar");
            UpdateProgressBar(1, 10);
        }
        public void UpdateProgressBar(int value, int maxValue)
        {
            ProgressBar.MaxValue = maxValue;
            ProgressBar.Value = value;

            //invisible if health full
            Visible = value != maxValue;

        }
    }
}

