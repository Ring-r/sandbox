#include <iostream>
#include <SDL2/SDL.h>

#include "base.h"

int main(){
	Base base;
	if(!base.Init()) {
		return 1;
	}

	SDL_Renderer* renderer = base.GetRenderer();

	SDL_Texture* texture = base.CreateTexture("./star.bmp");
	if(!texture) {
		return 1;
	}

	SDL_RenderClear(renderer);
	SDL_RenderCopy(renderer, texture, NULL, NULL);
	SDL_RenderPresent(renderer);

	SDL_Delay(2000);

	base.ReleaseTexture(texture);
}
