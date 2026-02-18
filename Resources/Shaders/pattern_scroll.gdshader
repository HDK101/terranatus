// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

void fragment() {
	vec4 color = texture(TEXTURE, vec2(UV.x + TIME * 0.2, UV.y));
	COLOR = color;
}