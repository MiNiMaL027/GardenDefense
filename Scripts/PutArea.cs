using Godot;
using System;

public partial class PutArea : Area3D
{
    private bool _isEnable = true;
	public bool isEnable {
        get
        {
            if (!_isEnable)
                Animation.Play("Disable");

            return _isEnable;
        }
        set 
        {
            _isEnable = value;
        }
    }
    public Vector3 SpawnPosition { get; set; }
    public AnimationPlayer Animation { get; set; }  
	public override void _Ready()
	{
        BodyEntered += PutArea_BodyEntered;
        BodyExited += PutArea_BodyExited;
        SpawnPosition = GetNode<Node3D>("SpawnPoint").GlobalPosition;
        Animation = GetNode<AnimationPlayer>("Animation");
	}

    private void PutArea_BodyExited(Node3D body)
    {
        if(body is Item)
        {
            isEnable = true;
        }
    }

    private void PutArea_BodyEntered(Node3D body)
    {
        if(body is Item)
        {
            isEnable = false;
        }
    }
}
