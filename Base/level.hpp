#ifndef LEVEL_H
#define LEVEL_H

#include "_.hpp"
#include "hero.hpp"

class Level {
private:
	float size_x, size_y;
	std::vector<Hero> heroes;
	uint8_t index;

	float screen_center_x, screen_center_y;

public:
	Level(uint8_t count = 1, uint8_t index = 0, bool random_init = false);
	~Level();

	void DoStep();
	void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;

	void SetScreenCenter(float screen_center_x, float screen_center_y);
};

const float DEFAULT_SIZE_X = 1000.0f;
const float DEFAULT_SIZE_Y = 1000.0f;

#endif // LEVEL_H
