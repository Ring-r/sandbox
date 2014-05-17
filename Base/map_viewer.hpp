#ifndef MAP_VIEWER_HPP
#define MAP_VIEWER_HPP

#include "_.hpp"
#include "map.hpp"

class MapViewer {
public:
	static const uint8_t DEFAULT_CELL_SIZE = 32;

private:
	uint32_t cell_size;
	std::vector<SDL_Texture*> textures;

public:
  MapViewer();
  ~MapViewer();

  void SetCellSize(uint8_t cell_size) {
    this->cell_size = cell_size;
  }

  uint32_t GetSizeX(const Map& map) const {
    return this->cell_size * map.count_x;
  }

  uint32_t GetSizeY(const Map& map) const {
    return this->cell_size * map.count_y;
  }

  void Load(const ViewerSdl& viewer, const std::string& filename);
  void Draw(SDL_Renderer* renderer, const Map& map, uint32_t screen_size_x, uint32_t screen_size_y, float x, float y, float angle) const;
};

#endif // MAP_VIEWER_HPP
