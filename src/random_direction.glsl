// Generate a pseudorandom unit 3D vector
// 
// Inputs:
//   seed  3D seed
// Returns psuedorandom, unit 3D vector drawn from uniform distribution over
// the unit sphere (assuming random2 is uniform over [0,1]²).
//
// expects: random2.glsl, PI.glsl
vec3 random_direction(vec3 seed)
{
  /////////////////////////////////////////////////////////////////////////////
  //Reference: https://www.scratchapixel.com/lessons/procedural-generation-virtual-worlds/perlin-noise-part-2/perlin-noise
  
  vec2 random_vector = random2(seed);

  float theta = acos(2 * random_vector.x - 1);
  float phi = 2 * M_PI * random_vector.y;

  return normalize(vec3(
    sin(phi) * cos(theta),
    sin(phi) * sin(theta),
    cos(phi)));
  /////////////////////////////////////////////////////////////////////////////
}
