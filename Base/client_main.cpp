#include "client.hpp"

int main(int argc, char * args[]) {
	const int count = 10;
	Client client;
	if(!client.Init(count)) {
		return 1;
	}
	client.Run();
}
