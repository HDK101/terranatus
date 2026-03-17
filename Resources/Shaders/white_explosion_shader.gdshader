// NOTE: Shader automatically converted from Godot Engine 4.6.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_add;

uniform sampler2D noise_sampler: repeat_enable, filter_nearest;
uniform sampler2D curve_sampler: filter_nearest;
uniform float current_radius: hint_range(0.0, 1.0, 0.01);

void fragment() {
	vec2 center = vec2(0.5f);
	float alpha = texture(noise_sampler, UV + vec2(TIME * 0.05f, 0.0f)).r;
	
	float dist_to_center = distance(UV, center) + (1.0f + sin(UV.x * 10.0f + TIME * PI)) * 0.01f;
	
	float max_limit = current_radius + 0.05f;
	float min_limit = current_radius - 0.05f;
	
	float ratio = smoothstep(min_limit, max_limit, dist_to_center);
	
	float curve = texture(curve_sampler, vec2(ratio, 0.0f)).r;
	
	if (dist_to_center <= max_limit && dist_to_center >= min_limit) {
		COLOR.a = alpha * curve;
	} else {
		COLOR.a = 0.0f;
	}
}