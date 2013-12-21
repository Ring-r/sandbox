#include <cmath>

float CalcZ(float x, float y) {
	return sqrt(1.0 - y * y - x * x);
}

struct Quaternion {
	float x, y, z, w;
};

void MulQuaternions(const Quaternion *q1, const Quaternion *q2, Quaternion *res) {
	float A = (q1->w + q1->x) * (q2->w + q2->x);
	float B = (q1->z - q1->y) * (q2->y - q2->z);
	float C = (q1->x - q1->w) * (q2->y + q2->z);
	float D = (q1->y + q1->z) * (q2->x - q2->w);
	float E = (q1->x + q1->z) * (q2->x + q2->y);
  	float F = (q1->x - q1->z) * (q2->x - q2->y);
	float G = (q1->w + q1->y) * (q2->w - q2->z);
	float H = (q1->w - q1->y) * (q2->w + q2->z);

	res->w = B + (-E - F + G + H) * 0.5f;
	res->x = A - ( E + F + G + H) * 0.5f; 
	res->y =-C + ( E - F + G - H) * 0.5f;
	res->z =-D + ( E - F - G + H) * 0.5f;
}

void QuaternionToMatrix(const Quaternion* q, float m[4][4]) {
	float x2 = q->x + q->x;
	float y2 = q->y + q->y;
	float z2 = q->z + q->z;

	float xx = q->x * x2;	float xy = q->x * y2;	float xz = q->x * z2;
	float yy = q->y * y2;	float yz = q->y * z2;	float zz = q->z * z2;
	float wx = q->w * x2;	float wy = q->w * y2;	float wz = q->w * z2;

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

	Quaternion q;
	q.x = sy * tz - ty * sz;
	q.y = - sx * tz + tx * sz;
	q.z = sx * ty - tx * sy;
	float cosa = sx * tx + sy * ty + sz * tz;
	float sina2 = sqrt(0.5 * (1.0f - cosa));
	float cosa2 = sqrt(0.5 * (1.0f + cosa));

	q.x *= sina2; q.y *= sina2; q.z *= sina2; q.w = cosa2;

	QuaternionToMatrix(&q, m);
}

void Fill(float sx, float sy, float tx, float ty, float sx_, float sy_, float tx_, float ty_, float m[4][4]) {
}