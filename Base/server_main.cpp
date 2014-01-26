#include "server.hpp"

int main(int argc, char * args[]) {
	const int count = 10;
	Server server;
	if(!server.Init(count)) {
		return 1;
	}
	server.Run();
}
