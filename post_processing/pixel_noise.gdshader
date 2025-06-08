shader_type canvas_item;
// If you intend to read from mipmaps with `textureLod()` LOD values greater than `0.0`,
// use `filter_nearest_mipmap` instead. This shader doesn't require it.
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

float random (vec2 uv) {
	return fract(sin(dot(uv.xy,
		vec2(12.9898,78.233))) * 43758.5453123);
}

void fragment() {
	vec2 pixel_position = SCREEN_UV * SCREEN_PIXEL_SIZE;
	vec4 screen_color = textureLod(screen_texture, SCREEN_UV, 0.0);
	float random_time_offset = random(vec2(TIME, TIME * 0.215));
	float random_x_offset = random(vec2(TIME * 0.18571, 0));
	float random_y_offset = random(vec2(0, TIME * 0.5192));
	float noise_value_raw = random(
		SCREEN_UV + vec2(random_x_offset, random_y_offset) * random_time_offset
	);
	float noise_value_ranged = mix(0.8, 1.0, noise_value_raw);
	vec4 final_color = screen_color * noise_value_ranged;
	COLOR = final_color;
}