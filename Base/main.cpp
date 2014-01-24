#include <iostream>
#include <SDL.h>
//#include <SDL2/SDL.h>

#include "base.h"

int _tmain(int argc, char * args[]) {
	Base base;
	if(!base.Init()) {
		return 1;
	}

	SDL_Texture* texture = base.CreateTexture("./star.bmp");
	if(!texture) {
		return 1;
	}

	bool quit = false;
	while(!quit) {
		SDL_Event sdl_event;
		while (SDL_PollEvent(&sdl_event)) {
			if (sdl_event.type == SDL_QUIT) {
				quit = true;
			}
			if (sdl_event.type == SDL_KEYDOWN) {
				quit = true;
			}
		}

		base.ClearRenderer();
		base.DrawTexture(texture);
		base.DrawRenderer();

		// SDL_Delay(2000);
	}

	base.ReleaseTexture(texture);
}
