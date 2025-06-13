shader_type spatial;

render_mode blend_premul_alpha;

uniform sampler2D main_tex : source_color, hint_default_white, filter_nearest;
uniform vec3 color : source_color = vec3(1,1,1);
instance uniform vec3 tint : source_color = vec3(1,1,1);
uniform sampler2D normal_tex : hint_normal, filter_nearest;
uniform sampler2D alpha_tex : hint_default_white, filter_nearest;
uniform float alpha : hint_range(0.0, 1.0) = 1.0;

void vertex() {
	// Called for every vertex the material is visible on.
	COLOR = vec4(color * tint * COLOR.rgb, 1.0);
}

void fragment() {
	vec4 main_tex_sample = texture(main_tex, UV);
	ALBEDO = main_tex_sample.rgb * COLOR.rgb;
	ROUGHNESS = 0.0;
	SPECULAR = 1.0;
	METALLIC = 0.0;
	
	NORMAL_MAP = texture(normal_tex, UV).rgb;
	
	vec4 alpha_tex_sample = texture(alpha_tex, UV);
	ALPHA = main_tex_sample.a * alpha * alpha_tex_sample.r;
	PREMUL_ALPHA_FACTOR = ALPHA;
}

void light() {
    DIFFUSE_LIGHT += clamp(
		smoothstep(0.25, 0.40, dot(NORMAL, LIGHT)), 0.0, 1.0
	) * ATTENUATION * LIGHT_COLOR / PI;
	DIFFUSE_LIGHT *= ALPHA;
}