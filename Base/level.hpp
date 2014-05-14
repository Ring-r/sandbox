#ifndef LEVEL_HPP
#define LEVEL_HPP

#include "_.hpp"
#include "map.hpp"

class Level {
private:
  Map map;

  float screen_center_x, screen_center_y;

public:
  Level();
  ~Level();

  void DoStep();
  void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;

  void LoadMap(const ViewerSdl& viewer, const std::string& filename);

  void SetScreenCenter(float screen_center_x, float screen_center_y);
};

#endif // LEVEL_HPP
