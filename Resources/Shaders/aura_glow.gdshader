// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_add;

uniform sampler2D noise_sampler: filter_nearest, repeat_enable;
uniform sampler2D circle_gradient: filter_nearest;
uniform float multiplier: hint_range(0.0, 1.0, 0.01) = 0.0f;

void fragment() {
	vec4 texture_color = texture(TEXTURE, UV);
	float noise_color = texture(noise_sampler, UV + vec2(0.0f, TIME * 0.2)).r;
	float circle_gradient_color = texture(circle_gradient, UV).r;
	
	float alpha = texture_color.a * multiplier * (noise_color) * (-0.5f + circle_gradient_color);
	
	vec4 color = texture_color;
	
	color.a = round(max(alpha, 0.0f) * 20.0f) / 20.0f;
	
	if (UV.y < 0.5f) {
		COLOR = color;
	} else {
		COLOR = vec4(0.0f);
	}
}