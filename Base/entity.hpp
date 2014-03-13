#ifndef ENTITY_H
#define ENTITY_H

#include "_.hpp"

class Entity {
public:
	float px, vx;
	float py, vy;
	float angle;
	float speed;

public: //CircleEntity
	float r;

public:
	Entity();
	~Entity();

	void DoStep();
};

void collision(Entity& entity_i, Entity& entity_j);

const float DEFAULT_ANGLE_STEP = 1.0f;
const float DEFAULT_RADIUS = 16.0f;
const float DEFAULT_SPEED = 1.0f;

#endif // ENTITY_H
