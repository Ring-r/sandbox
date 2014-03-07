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

public: // Entity
	uint16_t i;
	uint8_t j;

public:
	Entity();
	~Entity();

	void DoStep();
};

void collision(Entity& entity_i, Entity& entity_j);

const float DEFAULT_ANGLE_STEP = 3.0f;
const int DEFAULT_RADIUS = 16;
const int DEFAULT_SPEED = 3;

#endif // ENTITY_H
