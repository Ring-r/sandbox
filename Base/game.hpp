#ifndef GAME_HPP
#define GAME_HPP

#include "base.hpp"
#include "viewer_sdl.hpp"
#include "settings.hpp"

class Game : public Base, public ViewerSdl {
private:
	bool quit;
	Settings settings;

	void Event(const SDL_Event& sdl_event);

public:
	Game();
	~Game();

	void Run();
};

#endif // GAME_HPP
