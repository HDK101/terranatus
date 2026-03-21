// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

uniform sampler2D noise_texture: filter_nearest, repeat_enable;
uniform float progress: hint_range(0.0, 1.0, 0.01);

void fragment() {
	vec2 uv = UV + vec2(0.0f, sin(TIME + UV.x * 20.0f) * (1.1f - progress) * 0.2f);
	vec4 texture_color = texture(TEXTURE, uv);
	float threshold = texture(noise_texture, uv + vec2(TIME * 0.1f, 0.0f)).r;
	float white_min = threshold + 0.01;
	float white_max = threshold + 0.02;
	if (progress >= white_min && progress <= white_max) {
		vec4 color = vec4(vec3(1.0f), texture_color.a);
		COLOR = color;
	} else if (progress >= threshold) {
		COLOR = texture_color;
	} else {
		COLOR = vec4(0.0f);
	}
}