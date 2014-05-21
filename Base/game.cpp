#include "game.hpp"

#include <ctime>

#include "map.hpp"
#include "map_factory.hpp"
#include "map_viewer.hpp"

#include "bot_auto_1.hpp"
#include "bot_auto_2.hpp"

Game::Game() {
  SDL_LogSetAllPriority(SDL_LOG_PRIORITY_WARN); // TODO: Run only in debug?

	if(SDL_Init(SDL_INIT_EVERYTHING) < 0) {
		LogError("SDL_Init");
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
	SDL_Renderer* renderer = viewer.GetRenderer();

	int screen_size_x, screen_size_y;
	SDL_GetWindowSize(viewer.GetWindow(), &screen_size_x, &screen_size_y);

	Map map;
	MapFactory::InitsRandom(map, MapFactory::DEFAULT_COUNT_X, MapFactory::DEFAULT_COUNT_Y, MapFactory::DEFAULT_FILL_PERCENT);

	MapViewer mapViewer;
	mapViewer.SetCellSize(MapViewer::DEFAULT_CELL_SIZE);

	BotAuto1 bot_auto(0, mapViewer.GetSizeX(map), 0, mapViewer.GetSizeY(map));

	while(!quit) {
	  std::clock_t time = clock();

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
		}

		viewer.ClearViewer();

		// TODO: somes.DoStep();
		bot_auto.DoStep();

		if(renderer) {
		  // TODO: somes.Draw(viewer);
		  mapViewer.Draw(renderer, map, screen_size_x, screen_size_y, bot_auto.GetPx(), bot_auto.GetPy(), bot_auto.GetAngle());
		}

		viewer.EndDraw();

		const bool cap = true;
		const int frame_per_second = 60;
		int delay = 1000 / frame_per_second - (time - clock()) / CLOCKS_PER_SEC;
		if(cap && delay > 0) {
		  SDL_Delay(delay);
		}
	}
}
