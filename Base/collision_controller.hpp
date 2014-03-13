#ifndef COLLISION_CONTROLLER_H
#define COLLISION_CONTROLLER_H

#include "_.hpp"
#include "entity.hpp"

class CollisionController {
protected:
	uint32_t count_x, count_y, step_coef;
	std::vector<std::vector<Entity*>> cells;
	std::vector<uint32_t> indeces;

	void Updates(Entity& entity, uint32_t cell_index);

public:
	CollisionController();
	~CollisionController();

	void Updates(std::vector<Entity>& entities);
	void UpdatesOpt(std::vector<Entity>& entities);
};

static const int DEFAULT_STEP_COEF = 6;
static const int DEFAULT_COUNT_X = 10;
static const int DEFAULT_COUNT_Y = 10;

#endif // MAP_H
