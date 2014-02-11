#ifndef GAME_HPP
#define GAME_HPP

#include "base.hpp"
#include "viewer_sdl.hpp"
#include "settings.hpp"

class Game : public Base, public ViewerSdl {
private:
	bool quit;
	Settings settings;

public:
	Game();
	~Game();

	void Run();
};

#endif // GAME_HPP
