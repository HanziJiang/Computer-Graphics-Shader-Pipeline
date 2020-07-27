// Create a bumpy surface by using procedural noise to generate a height (
// displacement in normal direction).
//
// Inputs:
//   is_moon  whether we're looking at the moon or centre planet
//   s  3D position of seed for noise generation
// Returns elevation adjust along normal (values between -0.1 and 0.1 are
//   reasonable.
float bump_height( bool is_moon, vec3 s)
{
  /////////////////////////////////////////////////////////////////////////////
  float height;

  if (is_moon) {
    height = improved_perlin_noise(vec3(5, 4.5, 7) * s) * improved_perlin_noise(vec3(1, 3.5, 7) * s);
  } else {
    height = improved_perlin_noise(vec3(3.5, 7, 4.1) * s) * improved_perlin_noise(vec3(4.5, 8, 6.1) * s);
  }

  height *= 0.1;
  return height;
  /////////////////////////////////////////////////////////////////////////////
}
