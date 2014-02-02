#include "listener.hpp"

#include "base.hpp"
#include <algorithm>

Listener::Listener()
	: quit(false), listen_net_thread(&Listener::ListenNet, this) {
	this->commands["quit"] = &Listener::QuitCmd;
}

Listener::~Listener() {
	this->quit = true;
	this->listen_net_thread.join();
}

void Listener::ListenNet(int port) {
	UDPsocket socket = SDLNet_UDP_Open(port);
	if(!socket) {
		LogSdlError("SDLNet_UDP_Open");
		return;
	}

	UDPpacket* packet = SDLNet_AllocPacket(MAX_COMMAND_LENGTH);
	while(!this->quit) {
		if(SDLNet_UDP_Recv(socket, packet) > 0) {
			// TODO: lock
			// TODO: Run command(command.id, command.data, command.length);
			// TODO: unlock
		}
	}

	SDLNet_FreePacket(packet);
	SDLNet_UDP_Close(socket);
}

void Listener::Run() {
	while(!this->quit) {
		std::this_thread::sleep_for(std::chrono::seconds(1));
	}
}

void Listener::QuitCmd() {
	this->quit = true;
}
