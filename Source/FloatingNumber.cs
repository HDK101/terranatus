using Godot;
using System;
using System.Collections.Generic;

public partial class FloatingNumber : Node2D
{
	public bool IsEXP { get; set; } = false;

	private Texture2D texture;
	private Texture2D expTexture;
	private readonly float DEFAULT_VELOCITY_BASE = 64.0f;
	private readonly float GRAVITY_MULTIPLIER = 4.0f;

	private float velocityY;

	private string text;

	private List<Rect2> numberRects;

	private float fontScale = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		texture = GD.Load<Texture2D>("res://Fonts/boxy_number.png");
		expTexture = GD.Load<Texture2D>("res://Fonts/exp_text.png");
		velocityY = -DEFAULT_VELOCITY_BASE;
		GetTree().CreateTimer(2.0).Timeout += QueueFree;

		Vector2I fontSize = new(9, 10);

		fontScale = 3f;

		Tween tween = CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, nameof(fontScale), 1.0f, 0.5f);

		numberRects = [
			new Rect2(Vector2I.Zero, fontSize),
			new Rect2(Vector2I.Right * 1 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 2 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 3 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 4 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 5 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 6 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 7 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 8 * fontSize, fontSize),
			new Rect2(Vector2I.Right * 9 * fontSize, fontSize),
		];
	}

    public override void _Process(double delta)
    {
		velocityY += (float)delta * DEFAULT_VELOCITY_BASE * GRAVITY_MULTIPLIER;
        Position = new(Position.X, Position.Y + velocityY * (float)delta);
		QueueRedraw();
    }

    public override void _Draw()
    {
		int currentChar = 0;
		foreach (char c in text)
		{
			DrawNumber(c, new (currentChar * 9, 0), fontScale);
			currentChar += 1;
		}

		if (IsEXP)
		{
			currentChar += 1;
			Vector2 scaled = new(27 * fontScale, 10 * fontScale);
			DrawTextureRect(expTexture, new(new Vector2(currentChar * 9, 0) - scaled / 2, scaled), false);
		}
    }

	public void Start(string value)
	{
		text = value;
	}

	private void DrawNumber(char c, Vector2 position, float scale = 1f)
	{
		int number = (int)char.GetNumericValue(c);
		Vector2 scaled = new(9 * scale, 10 * scale);

		DrawTextureRectRegion(texture, new (position - scaled / 2, scaled), numberRects[number]);
	}
}
