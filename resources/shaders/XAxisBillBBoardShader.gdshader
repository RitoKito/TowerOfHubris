// NOTE: Shader automatically converted from Godot Engine 4.2.2.stable.mono's StandardMaterial3D.

shader_type spatial;
render_mode blend_mix,depth_draw_opaque,cull_back,diffuse_burley,specular_schlick_ggx;
uniform vec4 albedo : source_color;
uniform sampler2D texture_albedo : source_color,filter_nearest,repeat_enable;
uniform float point_size : hint_range(0,128);
uniform float roughness : hint_range(0,1);
uniform sampler2D texture_metallic : hint_default_white,filter_nearest,repeat_enable;
uniform vec4 metallic_texture_channel;
uniform sampler2D texture_roughness : hint_roughness_r,filter_nearest,repeat_enable;
uniform float specular;
uniform float metallic;
uniform vec3 uv1_scale;
uniform vec3 uv1_offset;
uniform vec3 uv2_scale;
uniform vec3 uv2_offset;


void vertex() {
	vec3 dir = CAMERA_POSITION_WORLD - NODE_POSITION_WORLD;
	dir.x = 0.0;
	dir = normalize(dir);
	
	vec3 forward = dir;
    vec3 right = vec3(1.0, 0.0, 0.0); // fixed right direction along X-axis
    vec3 up = cross(forward, right);
	
	mat3 bt = mat3(right, up, forward);

	VERTEX = bt * VERTEX;
}






void fragment() {
	vec2 base_uv = UV;
	vec4 albedo_tex = texture(texture_albedo,base_uv);
	ALBEDO = albedo_tex.rgb;
	ALPHA *= albedo_tex.a;
}
