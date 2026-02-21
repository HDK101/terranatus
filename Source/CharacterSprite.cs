using System;
using Godot;

public partial class CharacterSprite : AnimatedSprite2D
{
	private float damageWeight = 0.0f;

	public void Hit()
	{
		damageWeight = 0.5f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		byte colorByte = (byte)(255 - (int)(255 * damageWeight));
		Modulate = Color.Color8(255, colorByte, colorByte);

		damageWeight = MathF.Max(0.0f, damageWeight - (float)delta);
	}

	public Texture2D GetFrameTexture()
	{
		var frames = SpriteFrames;
		string animation = Animation;
		int frame = Frame;

		return frames.GetFrameTexture(animation, frame);
	}
}
