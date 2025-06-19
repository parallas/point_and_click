shader_type spatial;
render_mode unshaded;
render_mode cull_disabled;
render_mode blend_add;

uniform vec4 tint : source_color = vec4(1);
uniform float power : hint_range(0.01, 5.0) = 1;

void vertex() {
	COLOR = COLOR;
}

void fragment() {
	ALBEDO = tint.rgb;
	ALPHA = pow(COLOR.r, power) * tint.a;
}
