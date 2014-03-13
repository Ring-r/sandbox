#ifndef LEVEL_H
#define LEVEL_H

#include "_.hpp"
#include "map.hpp"
#include "entity.hpp"
#include "entity_controller.hpp"
#include "collision_controller.hpp"

class Level {
private:
	float screen_center_x, screen_center_y;

	Map map;

	uint32_t entity_index;
	std::vector<Entity> entities;
	EntityController entity_controller;

	CollisionController collision_controller;

	void RandomEntityChange();

public:
	Level();
	~Level();

	void DoStep();
	void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;

	void LoadMap(const ViewerSdl& viewer, const std::string& filename);

	void AddBots(uint8_t count, bool random_init = false);

	void SetScreenCenter(float screen_center_x, float screen_center_y);
};

#endif // LEVEL_H
