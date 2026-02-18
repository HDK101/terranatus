using Godot;
using System;

public partial class FlyingCorpse : Sprite2D
{
	public double RotationForceInRadians { get; set; }
	public Vector2 Velocity { get; set; }

	private Vector2 worldGravity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		float gravity = (float)ProjectSettings.GetSetting(
			"physics/2d/default_gravity"
		);

		Vector2 gravityDir = (Vector2)ProjectSettings.GetSetting(
			"physics/2d/default_gravity_vector"
		);

		worldGravity = gravityDir * gravity;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		Position += Velocity * deltaF;
		Velocity += worldGravity * (float)delta;
		Rotate((float)(RotationForceInRadians * delta));
	}

    public override void _ExitTree()
    {
        GD.Print("End!");
    }
}
