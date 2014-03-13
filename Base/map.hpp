#ifndef MAP_H
#define MAP_H

#include "_.hpp"
#include "entity.hpp"

class Map {
private:
	std::vector<SDL_Texture*> textures;

	float size_x, size_y;

	uint32_t count_x, count_y;
	std::vector<uint32_t> cells;

	void Clear();

public:
	Map(uint32_t count_x = 0, uint32_t count_y = 0);
	~Map();

	void Load(const ViewerSdl& viewer, const std::string& filename);
	void Draw(SDL_Renderer* renderer, const Entity& entity, float screen_center_x, float screen_center_y) const;

	void Updates(std::vector<Entity>& entity) const;

	void InitsRandom(std::vector<Entity>& entity) const;
};

const float DEFAULT_SIZE_X = 1000.0f;
const float DEFAULT_SIZE_Y = 1000.0f;
const uint8_t DEFAULT_BLOCK_SIZE = 32;

#endif // MAP_H
