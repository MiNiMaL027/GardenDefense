using Godot;
using System;
namespace Components
{
    public partial class ProgressBar3D : Sprite3D
    {
        public TextureProgressBar ProgressBar { get; set; }
        public override void _Ready()
        {
            this.Texture = GetNode<SubViewport>("Viewport").GetTexture();
            ProgressBar = GetNode<TextureProgressBar>("Viewport/ProgressBar");
            UpdateProgressBar(1, 10);
        }
        public void InitTexure(Texture2D backgroundTexture, Texture2D progressTexture)
        {
            ProgressBar.TextureProgress = progressTexture;
            ProgressBar.TextureUnder = backgroundTexture;
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

