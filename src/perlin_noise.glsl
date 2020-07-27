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
  vec3 gradient0 = random_direction(vec3(st_lower.x, st_lower.y, st_lower.z));
  vec3 gradient1 = random_direction(vec3(st_upper.x, st_lower.y, st_lower.z));
  vec3 gradient2 = random_direction(vec3(st_lower.x, st_upper.y, st_lower.z));
  vec3 gradient3 = random_direction(vec3(st_upper.x, st_upper.y, st_lower.z));
  vec3 gradient4 = random_direction(vec3(st_lower.x, st_lower.y, st_upper.z));
  vec3 gradient5 = random_direction(vec3(st_upper.x, st_lower.y, st_upper.z));
  vec3 gradient6 = random_direction(vec3(st_lower.x, st_upper.y, st_upper.z));
  vec3 gradient7 = random_direction(vec3(st_upper.x, st_upper.y, st_upper.z));
  
  // Vectors from each grid point to st
  vec3 distance0 = vec3(st_diff.x, st_diff.y, st_diff.z);
  vec3 distance1 = vec3(st_diff.x - 1, st_diff.y, st_diff.z);
  vec3 distance2 = vec3(st_diff.x, st_diff.y - 1, st_diff.z);
  vec3 distance3 = vec3(st_diff.x - 1, st_diff.y - 1, st_diff.z);
  
  vec3 distance4 = vec3(st_diff.x, st_diff.y, st_diff.z - 1);
  vec3 distance5 = vec3(st_diff.x - 1, st_diff.y, st_diff.z - 1);
  vec3 distance6 = vec3(st_diff.x, st_diff.y - 1, st_diff.z - 1);
  vec3 distance7 = vec3(st_diff.x - 1, st_diff.y - 1, st_diff.z - 1);
  
  vec3 smooth_st_diff = vec3(smooth_step(st_diff.x), smooth_step(st_diff.y), smooth_step(st_diff.z));

  // linear interpolation
  float a = mix(dot(gradient0, distance0), dot(gradient1, distance1), smooth_st_diff.x);
  float b = mix(dot(gradient2, distance2), dot(gradient3, distance3), smooth_st_diff.x);
  float c = mix(dot(gradient4, distance4), dot(gradient5, distance5), smooth_st_diff.x);
  float d = mix(dot(gradient6, distance6), dot(gradient7, distance7), smooth_st_diff.x);
  
  float e = mix(a, b, smooth_st_diff.y);
  float f = mix(c, d, smooth_st_diff.y);

  return mix(e, f, smooth_st_diff.z) / (sqrt(3)/2);
  /////////////////////////////////////////////////////////////////////////////
}

