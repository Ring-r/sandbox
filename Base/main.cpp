#include "game.hpp"

#undef main
int main(int argc, char* args[]) {
	Game game;
	game.Base::Init();
	game.ViewerSdl::Init("Test");
	game.Run();
	return 0;
}
