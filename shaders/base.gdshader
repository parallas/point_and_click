shader_type spatial;

uniform sampler2D main_tex : source_color, hint_default_white, filter_nearest;
uniform vec3 color : source_color = vec3(1,1,1);
uniform float vertex_color_intensity : hint_range(0, 1) = 1;
instance uniform vec3 tint : source_color = vec3(1,1,1);
uniform sampler2D normal_tex : hint_normal, filter_nearest;

void vertex() {
	// Called for every vertex the material is visible on.
	vec3 vertex_color = mix(vec3(1.0), COLOR.rgb, vertex_color_intensity);
	COLOR = vec4(color * tint * vertex_color, 1.0);
}

void fragment() {
	vec4 main_tex_sample = texture(main_tex, UV);
	ALBEDO = main_tex_sample.rgb * COLOR.rgb;
	ROUGHNESS = 0.0;
	SPECULAR = 1.0;
	METALLIC = 0.0;
	
	NORMAL_MAP = mix(texture(normal_tex, UV).rgb, vec3(0.5, 0.5, 1.0), 0.25);
}

void light() {
    DIFFUSE_LIGHT += clamp(
		smoothstep(0.25, 0.40, dot(NORMAL, LIGHT)), 0.0, 1.0
	) * ATTENUATION * LIGHT_COLOR / PI;
}