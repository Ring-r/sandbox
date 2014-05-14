#include "game.hpp"

//#include "level.hpp"
#include "map.hpp"
#include "map_factory.hpp"
#include "map_viewer.hpp"

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
	SDL_Renderer* renderer = viewer.GetRenderer();

	// TODO: Init somes.
	//SDL_Texture* texture = nullptr;//viewer.CreateTexture("./resources/entity.bmp");

	//Level level;
	//level.LoadMap(viewer, "./resources/map.txt");
	// level.AddBots(50, true);

	int w, h;
	SDL_GetWindowSize(viewer.GetWindow(), &w, &h);
	//level.SetScreenCenter(w >> 1, h >> 1);

	Map map;
	MapFactory::InitsRandom(map, MapFactory::DEFAULT_COUNT_X, MapFactory::DEFAULT_COUNT_Y, MapFactory::DEFAULT_FILL_PERCENT);

	MapViewer mapViewer;
	mapViewer.SetCellSize(MapViewer::DEFAULT_CELL_SIZE);

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
		}

		viewer.ClearViewer();

		// TODO: somes.DoStep();
		//level.DoStep();
		
		if(renderer) {
		        //level.Draw(renderer, texture);
			// TODO: somes.Draw(viewer);
MapFactory::InitsRandom(map, 1000, 1000, MapFactory::DEFAULT_FILL_PERCENT);
mapViewer.SetCellSize(1);
		  mapViewer.Draw(renderer, map, w, h, mapViewer.GetSizeX(map) >> 1, mapViewer.GetSizeY(map) >> 1);
		}

		viewer.EndDraw();
	}
	//client.Clear();
	//if(texture) {
	      //viewer.ReleaseTexture(texture);	texture = nullptr;
	//}
}
