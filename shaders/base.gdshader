shader_type spatial;

uniform vec3 color : source_color = vec3(1,1,1);
instance uniform vec3 tint : source_color = vec3(1,1,1);

void vertex() {
	// Called for every vertex the material is visible on.
	COLOR = vec4(color * tint * COLOR.rgb, 1.0);
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = COLOR.rgb;
	ROUGHNESS = 1.0;
	SPECULAR = 0.0;
	METALLIC = 0.0;
}

void light() {
    DIFFUSE_LIGHT += clamp(
		step(0, dot(NORMAL, LIGHT)), 0.0, 1.0
	) * ATTENUATION * LIGHT_COLOR / PI;

	//DIFFUSE_LIGHT += 1.0;
}
