#ifndef LEVEL_H
#define LEVEL_H

#include "_.hpp"
#include "hero.hpp"

class Level {
private:
	float size_x, size_y;
	std::vector<Hero> heroes;
	uint8_t index;

public:
	Level(uint8_t count = 1, uint8_t index = 0, bool random_init = false);
	~Level();

	void DoStep();
	void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;
};

const float DEFAULT_SIZE_X = 500.0f;
const float DEFAULT_SIZE_Y = 500.0f;

#endif // LEVEL_H
