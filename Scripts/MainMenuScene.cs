using Godot;
using System;

public partial class MainMenuScene : Node3D
{
    private float rotationSpeed = 0.05f;
    private float radius = 15f; // Відстань від камери до нульових координат
    private float currentRotation = 0.0f;

    Camera3D Camera { get; set; }
	public override void _Ready()
	{
		Camera = GetNode<Camera3D>("Camera3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        // Отримуємо поточний кут обертання камери навколо осі Z
        currentRotation += rotationSpeed * (float)delta;
        float x = Mathf.Cos(currentRotation) * radius;
        float z = Mathf.Sin(currentRotation) * radius;
        Camera.Transform = new Transform3D(Basis.Identity, new Vector3(x, 17, z));


        // Встановлюємо спрямування камери на нульові координати
        Camera.LookAt(Vector3.Zero, Vector3.Up);
    }
}
