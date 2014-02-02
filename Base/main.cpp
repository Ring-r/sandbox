#include "base.hpp"
#include "game.hpp"

int main(int argc, char* args[]) {
	Base base;
	if(base.Error()) {
		return 1;
	}

	Game game;
	game.Run();
	return 0;
}
