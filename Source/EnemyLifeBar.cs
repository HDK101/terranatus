using Godot;
using System;

public partial class EnemyLifeBar : Sprite2D
{
	private Sprite2D progress;
	private ShaderMaterial shaderMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		progress = GetNode<Sprite2D>("Progress");
		shaderMaterial = (ShaderMaterial)progress.Material;
	}

	public void Update(double value, double maxValue)
	{
		shaderMaterial.SetShaderParameter("ratio", (float)(value / maxValue));
	}
}
