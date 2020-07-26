// Set the pixel color using Blinn-Phong shading (e.g., with constant blue and
// gray material color) with a bumpy texture.
// 
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
//                     linearly interpolated from tessellation evaluation shader
//                     output
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
//               rgb color of this pixel
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  vec3 ka, kd, ks, n, v, l;
  float p, light_theta;
  mat4 light_orbit_matrix;
  vec4 d;

  light_theta = - animation_seconds / 4.0 * M_PI;
  light_orbit_matrix = mat4(
    cos(light_theta), 0.0, -sin(light_theta), 0.0,
    0.0,              1.0, 0.0,               0.0,
    sin(light_theta), 0.0, cos(light_theta),  0.0,
    0.0,              0.0, 0.0,               1.0);
  d = normalize(vec4(1.0, 1.0, 1.0, 1.0));
  d = light_orbit_matrix * d;

  float noise;
  if (is_moon) {
    ka = vec3(0.3, 0.3, 0.3);
    kd = vec3(0.3, 0.3, 0.3);
    ks = vec3(0.4, 0.4, 0.4);
    p = 1000;

    noise = improved_perlin_noise(vec3(2, 3.5, 10.8) * sphere_fs_in);
    kd = mix(kd, vec3(0.0, 0.0, 0.0), noise);
  } else {
    ka = vec3(0.3, 0.3, 0.3);
    kd = vec3(0.2627, 0.4118, 0.9451);
    ks = vec3(0.5, 0.5, 0.5);
    p = 70;

    noise = improved_perlin_noise(vec3(1, 2.5, 1.8) * sphere_fs_in);
    kd = mix(kd, vec3(1.0, 1.0, 1.0), noise);
  }

  n = normalize(normal_fs_in.xyz);
  v = normalize(pos_fs_in.xyz);
  l = normalize(d.xyz);

  float epsilon = 0.001;
  vec3 T, B;
  tangent(sphere_fs_in, T, B);
  vec3 position = bump_position(is_moon , sphere_fs_in);
  n = cross((bump_position(is_moon, sphere_fs_in + epsilon * T) - position) / epsilon, (bump_position(is_moon, sphere_fs_in + epsilon * B) - position) / epsilon);

  mat4 modeling_transformation = model(is_moon, animation_seconds);
  n = normalize((transpose(inverse(view * modeling_transformation)) * vec4(n, 1.0)).xyz);

  color = blinn_phong(ka, kd, ks, p, n, v, l);
  /////////////////////////////////////////////////////////////////////////////
}
