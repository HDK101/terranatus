using Godot;

public partial class BigWhiteExplosion : Sprite2D
{
	private ShaderMaterial shaderMaterial;

	public override void _Ready()
	{
		shaderMaterial = (ShaderMaterial)Material;

		Tween tween = CreateTween().SetTrans(Tween.TransitionType.Linear);

		tween.TweenMethod(Callable.From((float radius) => ChangeRadius(radius)), 0.0f, 1.0f, 1.0f);
		tween.TweenCallback(Callable.From(QueueFree));
	}

	private void ChangeRadius(float radius)
	{
		shaderMaterial.SetShaderParameter("current_radius", radius);
	}
}
