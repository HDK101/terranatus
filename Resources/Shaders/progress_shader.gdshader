// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

instance uniform float ratio: hint_range(0.0, 1.0, 0.01) = 0.5f;

void fragment() {
	vec4 color = texture(TEXTURE, UV);
	
	if (UV.x > ratio) {
		color.a = 0.0f;
	}
	
	COLOR = color;
}