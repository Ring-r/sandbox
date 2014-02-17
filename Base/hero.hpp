#ifndef HERO_H
#define HERO_H

#include "_.hpp"

class ViewerSdl;

class Hero {
private:
	const ViewerSdl* viewer;

	int x, y;
	SDL_Texture* texture;

	void Clear();

public:
	Hero();
	~Hero();

	void Init(const ViewerSdl* viewer);
	void Event(const SDL_Event& sdl_event);
	void DoStep();
};

#endif // HERO_H
