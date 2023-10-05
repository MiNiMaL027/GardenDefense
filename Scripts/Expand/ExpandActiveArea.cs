using Godot;

public partial class ExpandActiveArea : StaticBody3D
{
	public MeshInstance3D Instance { get; set; }
    public CollisionShape3D CollisionShape { get; set; }

	public bool isActive = false;
    public bool isRight = false;

	public override void _Ready()
	{
		Instance = GetNode<MeshInstance3D>("MeshInstance3D");
        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");

        MouseEntered += ExpandActiveArea_MouseEntered;
        MouseExited += ExpandActiveArea_MouseExited;
	}

    private void ExpandActiveArea_MouseExited()
    {
        if (isActive)
        {
            Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/Active.tres"));
        }
        else
        {
            Instance.Mesh.SurfaceSetMaterial(0, GD.Load<StandardMaterial3D>("res://Meterials/Expand/inactive.tres"));
        }
    }

    private void ExpandActiveArea_MouseEntered()
    {
        if(isActive)
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

    public void ToShow(bool isEnoughtMoney)
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

        if(X == 0)
        {
            Instance.Position += new Vector3(0, 0, Z / 2);
            CollisionShape.Position += new Vector3(0, 0, Z / 2);
        }
        else if(Z == 0)
        {
            Instance.Position += new Vector3(X/2, 0, 0);
            CollisionShape.Position += new Vector3(X / 2, 0, 0);
        }     
    }
}
