using Godot;
using System;

[Tool]
[GlobalClass]
public partial class FloatingTextEffect : RichTextEffect
{
	public string bbcode = "floating";
	
	private float maxTime = 2.0f;

    public override bool _ProcessCustomFX(CharFXTransform charFX)
    {
		float elapsedTime = (float)charFX.ElapsedTime;
		//int alpha = Math.Clamp((int)(Math.Clamp(charFX.ElapsedTime, 0.0, 1.0) * 255), 0, 255);

		float multiplier = Math.Max(0.0f, maxTime - elapsedTime) / maxTime;

		charFX.Offset = new(0.0f, MathF.Sin(elapsedTime * 5.0f + charFX.GlyphIndex * 0.05f) * 4.0f * multiplier);
		return true;
    }
}
