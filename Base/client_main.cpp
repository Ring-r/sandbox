#include "base.hpp"
#include "client.hpp"

int main(int argc, char* args[]) {
	Base base; // TODO: If some error then return.

	const int count = 10;
	Client client;
	if(!client.Init(count)) {
		return 1;
	}
	client.Run();
}
