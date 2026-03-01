// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

uniform sampler2D fog_texture: filter_nearest, repeat_enable;

void fragment() {
	vec4 color = texture(fog_texture, UV + vec2(TIME * 0.01f, 0.0f));
	
	if (color.r >= 0.5) {
		COLOR = vec4(0.1f, 0.1f, 0.3f, 0.3f);
	} else {
		COLOR = vec4(0.0f);
	}
}