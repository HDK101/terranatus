using Godot;

public partial class Shadow : Sprite2D
{
	public static readonly Color WHITE_FADE = new(1.0f, 1.0f, 1.0f, 0.0f);
	public Color InitialColor { get; set; }

	public override void _Ready()
	{
		Modulate = InitialColor;
		var tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "modulate", WHITE_FADE, 0.5f);
		tween.TweenCallback(Callable.From(QueueFree));
	}
}
