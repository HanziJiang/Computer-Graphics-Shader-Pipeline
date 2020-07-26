// Add (hard code) an orbiting (point or directional) light to the scene. Light
// the scene using the Blinn-Phong Lighting Model.
//
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
out vec3 color;
// expects: PI, blinn_phong
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  vec3 ka, kd, ks, n, v, l;
  float p, light_theta;
  mat4 light_orbit_matrix;
  vec4 d;

  light_theta = -animation_seconds / 5.0 * M_PI;
  light_orbit_matrix = mat4(
    cos(light_theta), 0.0, -sin(light_theta), 0.0,
    0.0,              1.0, 0.0,               0.0,
    sin(light_theta), 0.0, cos(light_theta),  0.0,
    0.0,              0.0, 0.0,               1.0);
  
  // Why d.w=1 does not work?
  d = normalize(vec4(1.0, 1.0, 1.0, 0.0));
  d = view * light_orbit_matrix * d;

  if (is_moon) {
    ka = vec3(0.3, 0.3, 0.3);
    kd = vec3(0.3, 0.3, 0.3);
    ks = vec3(0.4, 0.4, 0.4);
    p = 300;
  } else {
    ka = vec3(0.3, 0.3, 0.3);
    kd = vec3(0.1, 0.3, 1.0);
    ks = vec3(0.5, 0.5, 0.5);
    p = 200;
  } 

  n = normalize(normal_fs_in.xyz);
  v = normalize(-view_pos_fs_in.xyz);
  l = normalize(d.xyz);

  color = blinn_phong(ka, kd, ks, p, n, v, l);
  /////////////////////////////////////////////////////////////////////////////
}
