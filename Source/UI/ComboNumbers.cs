using Godot;
using System;

public partial class ComboNumbers : Control
{
	public string Text
	{
		get => _text; set
		{
			_text = value;
			QueueRedraw();
		}
	}
    private string _text = "";

	private Texture2D zero;
	private Texture2D one;
	private Texture2D two;
	private Texture2D three;
	private Texture2D four;
	private Texture2D five;
	private Texture2D six;
	private Texture2D seven;
	private Texture2D eight;
	private Texture2D nine;

	public override void _Ready()
	{
		zero = GD.Load<Texture2D>("res://Sprites/Numbers/combo-zero.png");
		one = GD.Load<Texture2D>("res://Sprites/Numbers/combo-one.png");
		two = GD.Load<Texture2D>("res://Sprites/Numbers/combo-two.png");
		three = GD.Load<Texture2D>("res://Sprites/Numbers/combo-three.png");
		four = GD.Load<Texture2D>("res://Sprites/Numbers/combo-four.png");
		five = GD.Load<Texture2D>("res://Sprites/Numbers/combo-five.png");
		six = GD.Load<Texture2D>("res://Sprites/Numbers/combo-six.png");
		seven = GD.Load<Texture2D>("res://Sprites/Numbers/combo-seven.png");
		eight = GD.Load<Texture2D>("res://Sprites/Numbers/combo-eight.png");
		nine = GD.Load<Texture2D>("res://Sprites/Numbers/combo-nine.png");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	public override void _Draw()
	{
		int charX = 0;
		foreach (char c in _text)
		{
			var charPosition = new Vector2(charX * 24, Mathf.Sin(Time.GetTicksMsec() / 1000.0f + charX) * 4);
			Texture2D charTex = null;
			switch (c)
			{
				case '0':
					charTex = zero;
					break;
				case '1':
					charTex = one;
					break;
				case '2':
					charTex = two;
					break;
				case '3':
					charTex = three;
					break;
				case '4':
					charTex = four;
					break;
				case '5':
					charTex = five;
					break;
				case '6':
					charTex = six;
					break;
				case '7':
					charTex = seven;
					break;
				case '8':
					charTex = eight;
					break;
				case '9':
					charTex = nine;
					break;
			}
			DrawTexture(charTex, charPosition);
			charX += 1;
		}
	}

	private Tween CreateDefaultTween()
	{
		var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		return tween;
	}
}
