#include <cmath>

float CalcZ(float x, float y) {
	return sqrt(1.0 - y * y - x * x);
}

void QuaternionToMatrix(float x, float y, float z, float w, float m[4][4]) {
  float x2 = x + x;		float y2 = y + y;	float z2 = z + z;
  float xx = x * x2;	float xy = x * y2;	float xz = x * z2;
  float yy = y * y2;	float yz = y * z2;	float zz = z * z2;
  float wx = w * x2;	float wy = w * y2;	float wz = w * z2;

  m[0][0] = 1.0f - (yy + zz); m[0][1] = xy - wz; m[0][2] = xz + wy;
  m[1][0] = xy + wz; m[1][1] = 1.0f - (xx + zz); m[1][2] = yz - wx;
  m[2][0] = xz - wy; m[2][1] = yz + wx; m[2][2] = 1.0f - (xx + yy);

  m[0][3] = m[1][3] = m[2][3] = 0;
  m[3][0] = m[3][1] = m[3][2] = 0;
  m[3][3] = 1;
}

void Fill(float sx, float sy, float tx, float ty, float m[4][4]) {
	float sz = CalcZ(sx, sy);
	float tz = CalcZ(tx, ty);

	float x = sy * tz - ty * sz;
	float y = - sx * tz + tx * sz;
	float z = sx * ty - tx * sy;
	float cosa = sx * tx + sy * ty + sz * tz;

	float sina2 = sqrt(0.5 * (1.0f - cosa));
	float cosa2 = sqrt(0.5 * (1.0f + cosa));

	float w = cosa2; x *= sina2; y *= sina2; z *= sina2;

	QuaternionToMatrix(x, y, z, w, m);
}

void Fill(float sx, float sy, float tx, float ty, float sx_, float sy_, float tx_, float ty_, float m[4][4]) {
}