shader_type canvas_item;
// If you intend to read from mipmaps with `textureLod()` LOD values greater than `0.0`,
// use `filter_nearest_mipmap` instead. This shader doesn't require it.
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

uniform float min_brightness = 0.8;
uniform float max_brightness = 1.0;

float random (vec2 uv) {
	return fract(sin(dot(uv.xy,
		vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
	float vignette_amount = mix(1.0, 0.0, distance(vec2(0.5, 0.5), SCREEN_UV));
	
	vec4 screen_color = textureLod(screen_texture, SCREEN_UV, 0.0);
	float random_time_offset = random(vec2(TIME, TIME * 0.215));
	float random_x_offset = random(vec2(TIME * 0.18571, 0));
	float random_y_offset = random(vec2(0, TIME * 0.5192));
	float noise_value_raw = random(
		SCREEN_UV + vec2(random_x_offset, random_y_offset) * random_time_offset
	);
	float noise_value_ranged = mix(
		mix(
			0,
			min_brightness,
			pow(vignette_amount, 0.75)
		),
		max_brightness,
		noise_value_raw
	);
	vec3 final_color = screen_color.rgb;
	final_color *= noise_value_ranged;
	final_color *= mix(0.2, 1.0, vignette_amount);
	COLOR = vec4(final_color, 1.0);
}