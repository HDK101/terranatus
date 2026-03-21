shader_type canvas_item;


uniform float offset: hint_range(0.0, 10.0);
uniform vec4 color: source_color;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear;

void fragment() {
	vec2 uv = UV;
	vec2 halfpixel = SCREEN_PIXEL_SIZE / 2.0;
	
	vec4 sum = texture(SCREEN_TEXTURE, uv + vec2(-halfpixel.x * 2.0, 0.0) * offset);
	sum += texture(SCREEN_TEXTURE, uv + vec2(-halfpixel.x, halfpixel.y) * offset) * 2.0;
	sum += texture(SCREEN_TEXTURE, uv + vec2(0.0, halfpixel.y * 2.0) * offset);
	sum += texture(SCREEN_TEXTURE, uv + vec2(halfpixel.x, halfpixel.y) * offset) * 2.0;
	sum += texture(SCREEN_TEXTURE, uv + vec2(halfpixel.x * 2.0, 0.0) * offset);
	sum += texture(SCREEN_TEXTURE, uv + vec2(halfpixel.x, -halfpixel.y) * offset) * 2.0;
	sum += texture(SCREEN_TEXTURE, uv + vec2(0.0, -halfpixel.y * 2.0) * offset);
	sum += texture(SCREEN_TEXTURE, uv + vec2(-halfpixel.x, -halfpixel.y) * offset) * 2.0;

	COLOR = mix(sum / 12.0, color, 0.75f);
}
