// Generate a procedural planet and orbiting moon. Use layers of (improved)
// Perlin noise to generate planetary features such as vegetation, gaseous
// clouds, mountains, valleys, ice caps, rivers, oceans. Don't forget about the
// moon. Use `animation_seconds` in your noise input to create (periodic)
// temporal effects.
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
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  vec3 ka, kd, ks, n, v, l;
  float p, light_theta;
  mat4 light_orbit_matrix;
  vec4 d;

  // Generate light
  light_theta = - animation_seconds / 4.0 * M_PI;
  light_orbit_matrix = mat4(
    cos(light_theta), 0.0, -sin(light_theta), 0.0,
    0.0,              1.0, 0.0,               0.0,
    sin(light_theta), 0.0, cos(light_theta),  0.0,
    0.0,              0.0, 0.0,               1.0);
  d = normalize(vec4(1.0, 1.0, 1.0, 0.0));
  d = view * light_orbit_matrix * d;

  float noise;
  if (is_moon) {
    ka = vec3(0.0118, 0.0941, 0.1412);
    kd = vec3(0.9451, 0.9176, 0.8078);
    ks = vec3(0.0196, 0.2784, 0.3255);
    p = 10;

    noise = improved_perlin_noise(vec3(2, 3.5, 10.8) * sphere_fs_in);
    ka = mix(ka, vec3(0.1216, 0.2275, 0.0745), noise);
    
    noise = improved_perlin_noise(vec3(2, 5.5, 3.8) * sphere_fs_in);
    kd = mix(vec3(1.0, 1.0, 1.0), kd, noise);

    noise = improved_perlin_noise(vec3(1.1, 1.5, 1.3) * sphere_fs_in);
    kd = mix(vec3(0.0118, 0.2314, 0.2314), kd, noise);
    
    noise = improved_perlin_noise(vec3(12.75, 13.322, 10.8) * sphere_fs_in);
    ks = mix(ks, vec3(0.5216, 0.4549, 0.098), noise);

    if (bump_height(is_moon, sphere_fs_in) < 0) {
      kd = mix(vec3(0.0, 0.0, 0.0), kd, 0.3);
    } 
  } else {
    ka = vec3(0.1137, 0.1059, 0.1059);
    kd = vec3(0.8745, 0.3608, 0.1255);
    ks = vec3(0.5, 0.5, 0.5);
    p = 70;

    noise = improved_perlin_noise(vec3(1.22, 7.11, 1.8) * sphere_fs_in);
    kd = mix(kd, vec3(0.2275, 0.1373, 0.1373), noise + (1-noise) * 0.3);

    noise = improved_perlin_noise(vec3(1.22, 18.11, 1.8) * sphere_fs_in);
    kd = mix(kd, vec3(1, 1, 1), noise);

    // Create cloud
    float cloud = improved_perlin_noise(vec3(0.98, 9, 1) * (rotate_about_y(animation_seconds) * vec4(sphere_fs_in, 1.0)).xyz);
    if (cloud > 0.2){
      kd = mix(kd, vec3(0.9686, 0.8667, 0.8667), cloud + (1 - cloud) * 0.7);
    }
  }

  n = normalize(normal_fs_in.xyz);
  v = normalize(-view_pos_fs_in.xyz);
  l = normalize(d.xyz);

  // Create bumpy surface
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
