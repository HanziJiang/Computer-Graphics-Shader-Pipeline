// Input:
//   N  3D unit normal vector
// Outputs:
//   T  3D unit tangent vector
//   B  3D unit bitangent vector
void tangent(in vec3 N, out vec3 T, out vec3 B)
{
  /////////////////////////////////////////////////////////////////////////////
  // Reference: https://answers.unity.com/questions/133680/how-do-you-find-the-tangent-from-a-given-normal.html

  vec3 t1 = cross(N, vec3(0, 0, 1));
  vec3 t2 = cross(N, vec3(0, 1, 0));
  T = (length(t1) > length(t2)) ? normalize(t1) : normalize(t2);

  B = normalize(cross(T, N));
  /////////////////////////////////////////////////////////////////////////////
}
