// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

uniform float progress: hint_range(0.0, 1.0, 0.01);
uniform sampler2D noise_texture: filter_nearest, repeat_enable;
uniform sampler2D gradient_texture: filter_nearest, repeat_enable;
uniform vec4 color: source_color;

void fragment() {
	float noise_alpha_value = texture(noise_texture, UV + vec2(TIME * 0.1f, TIME * 0.2f)).r;
	float gradient_alpha_value = texture(gradient_texture, UV).r;
	
	float alpha = round((progress - noise_alpha_value - gradient_alpha_value)* 2.0f) / 2.0f;
	
	COLOR = vec4(vec3(color.rgb), alpha);
}