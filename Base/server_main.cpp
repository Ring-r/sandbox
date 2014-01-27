#include "base.hpp"
#include "server.hpp"

int main(int argc, char* args[]) {
	Base base; // TODO: If some error then return.

	const int count = 10;
	Server server;
	if(!server.Init(count)) {
		return 1;
	}
	server.Run();
}
