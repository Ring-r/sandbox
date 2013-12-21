#include <cfloat>

#include "utils.h"

const int DATA_COUNT = 10;
const float DATA_MIN = -100;
const float DATA_MAX = +100;

const int dim = 3;
GLfloat min[dim];
GLfloat max[dim];
GLfloat size[dim];
GLfloat center[dim];
GLfloat x, y, z, r;

int count = 0;
GLfloat* buffer0 = 0;

void UpdateMinMax() {
	for(int i = 0; i < dim; ++i) {
		min[i] = FLT_MAX;
		max[i] = FLT_MIN;
	}
	int coor_i = -1;
	for(int i = 0; i < bufferCount; ++i) {
		for(int j = 0; j < dim; ++j) {
			GLfloat coor = buffer[++coor_i];
			if(min[j] > coor) {
				min[j] = coor;
			}
			if(max[j] < coor) {
				max[j] = coor;
			}
		}
	}
	for(int i = 0; i < dim; ++i) {
		center[i] = 0.5f * (max[i] - min[i]);
		center[i] = 0.5f * (min[i] + max[i]);
	}
	x = center[0];
	y = center[1];
	z = center[2];	
}

void ClearData() {
	if (buffer) {
		delete[] buffer;
		bufferCount = 0;		
	}
}

void CreateData() {
	ClearData();

	bufferCount = 4;

	buffer = new GLfloat[dim * bufferCount];
	buffer[ 0] = 0.0f; buffer[ 1] = 0.0f; buffer[ 2] = 0.0f;
	buffer[ 3] = 1.0f; buffer[ 4] = 0.0f; buffer[ 5] = 0.0f;
	buffer[ 6] = 0.0f; buffer[ 7] = 1.0f; buffer[ 8] = 0.0f;
	buffer[ 9] = 0.0f; buffer[10] = 0.0f; buffer[11] = 1.0f;

	colors = new GLfloat[dim * bufferCount];
	colors[ 0] = 1.0f; colors[ 1] = 1.0f; colors[ 2] = 1.0f;
	colors[ 3] = 1.0f; colors[ 4] = 0.0f; colors[ 5] = 0.0f;
	colors[ 6] = 0.0f; colors[ 7] = 1.0f; colors[ 8] = 0.0f;
	colors[ 9] = 0.0f; colors[10] = 0.0f; colors[11] = 1.0f;

	buffer = new GLint[dim * bufferCount];
	buffer[ 0] = 0; buffer[ 1] = 1; buffer[ 2] = 2;
	buffer[ 3] = 0; buffer[ 4] = 2; buffer[ 5] = 3;
	buffer[ 6] = 0; buffer[ 7] = 3; buffer[ 8] = 1;
	buffer[ 9] = 1; buffer[10] = 2; buffer[11] = 3;

	x = y = z = 0.0f;
	r = 1.0f;

	//UpdateMinMax();
}


void CreateRandomData() {
	ClearData();
	bufferCount = DATA_COUNT;
	buffer = new GLfloat[dim * bufferCount];
	int coor_i = -1;
	for(int i = 0; i < bufferCount; ++i) {
		buffer[++coor_i] = static_cast<GLfloat>(RandomFloat(DATA_MIN, DATA_MAX));
		buffer[++coor_i] = static_cast<GLfloat>(RandomFloat(DATA_MIN, DATA_MAX));
		buffer[++coor_i] = static_cast<GLfloat>(RandomFloat(DATA_MIN, DATA_MAX));
	}
	UpdateMinMax();
}

void DrawData() {
	glColor3f(1.0f, 1.0f, 1.0f);
	glBegin(GL_TRIANGLES);
	int v, coor_i = -1;
	for(int i = 0; i < bufferCount; ++i) {
		++coor_i; v = dim * buffer[coor_i];
		glColor3f(colors[v], colors[v + 1], colors[v + 2]);
		glVertex3f(buffer[v], buffer[v + 1], buffer[v + 2]);

		++coor_i; v = dim * buffer[coor_i];
		glColor3f(colors[v], colors[v + 1], colors[v + 2]);
		glVertex3f(buffer[v], buffer[v + 1], buffer[v + 2]);

		++coor_i; v = dim * buffer[coor_i];
		glColor3f(colors[v], colors[v + 1], colors[v + 2]);
		glVertex3f(buffer[v], buffer[v + 1], buffer[v + 2]);
	}
	glEnd();
}