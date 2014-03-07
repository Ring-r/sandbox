#ifndef MAP_H
#define MAP_H

#include "_.hpp"
#include "entity.hpp"

class Map {
protected:
	uint8_t count_x, count_y, step_coef;
	std::vector<std::vector<Entity*>> cells;

public:
	Map();
	~Map();

	void Inits(Entity* entity);
	void Updates(Entity* entity);
	void CheckCollision(Entity* entity);
};

static const int DEFAULT_STEP_COEF = 5;
static const int DEFAULT_COUNT_X = 100;
static const int DEFAULT_COUNT_Y = 100;

#endif // MAP_H
