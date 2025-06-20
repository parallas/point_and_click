shader_type spatial;

#include "shading.gdshaderinc"

group_uniforms Albedo;
uniform sampler2D main_tex : source_color, hint_default_white, filter_nearest;
uniform vec3 color : source_color = vec3(1,1,1);
uniform float vertex_color_intensity : hint_range(0, 1) = 1;
instance uniform vec3 tint : source_color = vec3(1,1,1);

group_uniforms Metallic;
uniform sampler2D metallic_tex : hint_default_white, filter_nearest;
uniform float metallic_intensity : hint_range(0.0, 1.0) = 0;

group_uniforms Roughness;
uniform sampler2D roughness_tex : hint_default_black, filter_nearest;
uniform float roughness_intensity : hint_range(0.0, 1.0) = 1;

group_uniforms NormalMap;
uniform sampler2D normal_tex : hint_normal, filter_nearest;

group_uniforms Emission;
uniform sampler2D emissive_tex : hint_default_black, filter_nearest;
uniform float emissive_intensity = 1.0;

group_uniforms Matcap;
uniform sampler2D matcap_tex : hint_default_transparent, filter_nearest;
uniform float matcap_intensity = 1.0;
uniform float matcap_power = 1.0;

varying vec3 v_world_normal;
varying vec3 v_world_pos;
varying vec3 v_eye_pos;

varying mat3 TBN; // from view to tangent space
varying mat3 INV_TBN; // from tangent to view space

void vertex() {
	// Called for every vertex the material is visible on.
	vec3 vertex_color = mix(vec3(1.0), COLOR.rgb, vertex_color_intensity);
	COLOR = vec4(color * tint * vertex_color, 1.0);
	
	//v_world_normal = (MODEL_NORMAL_MATRIX * NORMAL);
	v_world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	v_eye_pos = (INV_VIEW_MATRIX * vec4(EYE_OFFSET, 1.0)).xyz;
	vec4 worldPos = (MODEL_MATRIX * vec4(VERTEX, 1.0));
	v_world_pos = worldPos.xyz;
	
	mat4 m = transpose(inverse(MODELVIEW_MATRIX));
    TBN = mat3(
        (m * vec4(TANGENT, 1.0)).xyz, // from model to view space
		(m * vec4(BINORMAL, 1.0)).xyz,
		-(m * vec4(NORMAL, 1.0)).xyz
    );
    INV_TBN = inverse(TBN);
}

void fragment() {
	vec4 main_tex_sample = texture(main_tex, UV);
	vec3 base_color = main_tex_sample.rgb * COLOR.rgb;
	ROUGHNESS = 0.0;
	SPECULAR = 1.0;
	METALLIC = 0.0;
	
	vec3 normal_sample = texture(normal_tex, UV).rgb;
	vec3 normal_adjusted = mix(normal_sample, vec3(0.5, 0.5, 1.0), 0.25);
	NORMAL_MAP = normal_adjusted;
	
	vec4 emissive_sample = texture(emissive_tex, UV);
	EMISSION = emissive_sample.rgb * emissive_intensity;
	
	float metallic_sample = texture(metallic_tex, UV).r;
	float metallic_value = metallic_sample * metallic_intensity;
	
	float roughness_sample = texture(roughness_tex, UV).r;
	float roughness_value = roughness_sample * roughness_intensity;
	float smoothness_value = 1.0 - roughness_value;
	
	vec3 normalmap = normal_sample * vec3(2.0) - vec3(1.0);
	vec3 normal_world = normalize(TANGENT * normalmap.y + BINORMAL * normalmap.x + NORMAL * normalmap.z);
	v_world_normal = (INV_VIEW_MATRIX * vec4(normal_world, 0.0)).rgb;
	vec2 matcap_uv = get_matcap_uv(v_world_normal, v_eye_pos, v_world_pos, 0.15, false);
	vec4 matcap_color = texture(matcap_tex, matcap_uv);
    vec4 matcap_adjusted = pow(mix(vec4(0), matcap_color, matcap_intensity), vec4(matcap_power));
	float matcap_alpha = matcap_adjusted.a;
	float matcap_visibility = matcap_alpha * smoothness_value;
	vec3 matcap_add = mix(vec3(0), matcap_adjusted.rgb, matcap_visibility) * 0.05;
	vec3 matcap_mul = mix(vec3(1), matcap_adjusted.rgb, matcap_visibility);
	ALBEDO = mix(base_color + matcap_add, base_color * matcap_mul, float(metallic_value));
}

void light() {
    DIFFUSE_LIGHT += clamp(
		smoothstep(0.25, 0.40, dot(NORMAL, LIGHT)), 0.0, 1.0
	) * ATTENUATION * LIGHT_COLOR / PI;
}