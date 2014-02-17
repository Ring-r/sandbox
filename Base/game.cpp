#include "game.hpp"

Game::Game() {
	if(SDL_Init(SDL_INIT_EVERYTHING) < 0) {
		LogSdlError("SDL_Init");
		return;
	}
	if(SDLNet_Init() < 0) {
		LogSdlError("SDLNet_Init");
		return;
	}
	if(TTF_Init() < 0) {
		LogTtfError("TTF_Init");
		return;
	}
}

Game::~Game() {
	SDLNet_Quit();
	SDL_Quit();
}

void Game::Run() {
	bool quit = false;
	Settings settings;
	ViewerSdl viewer; viewer.Init(settings.title);

	// TODO: Init somes.

	Hero hero; hero.Init(&viewer);

	while(!quit) {
		SDL_Event sdl_event;
		while(SDL_PollEvent(&sdl_event)) {
			if(sdl_event.type == SDL_QUIT) {
				quit = true;
			}
			if(sdl_event.type == SDL_KEYDOWN) {
				if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
					quit = true;
				}
			}

			// TODO: somes.Event(sdl_event);
			hero.Event(sdl_event);
		}

		viewer.ClearViewer();

		// TODO: somes.DoStep();
		hero.DoStep();

		viewer.EndDraw();
	}
	//client.Clear();
}
