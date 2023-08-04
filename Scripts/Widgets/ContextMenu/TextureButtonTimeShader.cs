using Godot;
using System;

public partial class TextureButtonTimeShader : TextureButton
{
    float time = 0f;
    public override void _Process(double delta)
    {
        if (Material != null && Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("time", time);
            time += (float)delta;
        }
        
    }
    public void SetShaderMaterial(ShaderMaterial shaderMaterial)
    {
        Material= shaderMaterial;
        time = 0f;
    }
}
