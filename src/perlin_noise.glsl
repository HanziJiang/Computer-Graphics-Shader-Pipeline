// Given a 3d position as a seed, compute a smooth procedural noise
// value: "Perlin Noise", also known as "Gradient noise".
//
// Inputs:
//   st  3D seed
// Returns a smooth value between (-1,1)
//
// expects: random_direction, smooth_step
float perlin_noise(vec3 st)
{
  /////////////////////////////////////////////////////////////////////////////
  // Reference: https://www.scratchapixel.com/lessons/procedural-generation-virtual-worlds/perlin-noise-part-2/perlin-noise

  vec3 st_lower = mod(floor(st), 255);
  vec3 st_upper = mod(st_lower + 1, 255);
  vec3 st_diff = st - floor(st);
  
  // Pseudorandom unit 3D vector starting from each grid point
  vec3 c000 = random_direction(vec3(st_lower.x, st_lower.y, st_lower.z));
  vec3 c100 = random_direction(vec3(st_upper.x, st_lower.y, st_lower.z));
  vec3 c010 = random_direction(vec3(st_lower.x, st_upper.y, st_lower.z));
  vec3 c110 = random_direction(vec3(st_upper.x, st_upper.y, st_lower.z));
  vec3 c001 = random_direction(vec3(st_lower.x, st_lower.y, st_upper.z));
  vec3 c101 = random_direction(vec3(st_upper.x, st_lower.y, st_upper.z));
  vec3 c011 = random_direction(vec3(st_lower.x, st_upper.y, st_upper.z));
  vec3 c111 = random_direction(vec3(st_upper.x, st_upper.y, st_upper.z));
  
  // Vectors from each grid point to st
  vec3 p000 = vec3(st_diff.x, st_diff.y, st_diff.z);
  vec3 p100 = vec3(st_diff.x - 1, st_diff.y, st_diff.z);
  vec3 p010 = vec3(st_diff.x, st_diff.y - 1, st_diff.z);
  vec3 p110 = vec3(st_diff.x - 1, st_diff.y - 1, st_diff.z);
  
  vec3 p001 = vec3(st_diff.x, st_diff.y, st_diff.z - 1);
  vec3 p101 = vec3(st_diff.x - 1, st_diff.y, st_diff.z - 1);
  vec3 p011 = vec3(st_diff.x, st_diff.y - 1, st_diff.z - 1);
  vec3 p111 = vec3(st_diff.x - 1, st_diff.y - 1, st_diff.z - 1);
  
  vec3 smooth_st_diff = vec3(smooth_step(st_diff.x), smooth_step(st_diff.y), smooth_step(st_diff.z));

  // linear interpolation
  float a = mix(dot(c000, p000), dot(c100, p100), smooth_st_diff.x);
  float b = mix(dot(c010, p010), dot(c110, p110), smooth_st_diff.x);
  float c = mix(dot(c001, p001), dot(c101, p101), smooth_st_diff.x);
  float d = mix(dot(c011, p011), dot(c111, p111), smooth_st_diff.x);
  
  float e = mix(a, b, smooth_st_diff.y);
  float f = mix(c, d, smooth_st_diff.y);

  return mix(e, f, smooth_st_diff.z) / (sqrt(3)/2);
  /////////////////////////////////////////////////////////////////////////////
}

