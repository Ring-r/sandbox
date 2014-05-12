#ifndef MAP_VIEWER_HPP
#define MAP_VIEWER_HPP

#include "_.hpp"
#include "map.hpp"
#include "entity.hpp"

class MapViewer {
public:
	static const uint8_t DEFAULT_CELL_SIZE = 32;

private:
	uint32_t cell_size;
	std::vector<SDL_Texture*> textures;

public:
	MapViewer();
	~MapViewer();

	void Load(const ViewerSdl& viewer, const std::string& filename);
	void Draw(SDL_Renderer* renderer, const Map& map, const Entity& entity, float screen_center_x, float screen_center_y) const;
};

#endif // MAP_VIEWER_HPP
