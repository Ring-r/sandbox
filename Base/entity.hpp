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

public: // MapEntity
	uint16_t i;
	uint8_t j;

public:
	Entity();
	~Entity();

	void DoStep();
};

void collision(Entity& entity_i, Entity& entity_j);

const float DEFAULT_ANGLE_STEP = 3.0f;
const float DEFAULT_RADIUS = 16.0f;
const float DEFAULT_SPEED = 3.0f;

#endif // ENTITY_H
