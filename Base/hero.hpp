#ifndef HERO_H
#define HERO_H

#include "entity.hpp"

class Hero : public Entity {
public:
	void DoStep();
	void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;
};

#endif // HERO_H
