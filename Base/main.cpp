#include "base.hpp"
#include "game.hpp"

int main(int argc, char* args[]) {
	Base base; base.Init();
	Game game; game.Run();
	return 0;
}
