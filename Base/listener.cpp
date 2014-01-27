#include "listener.hpp"

#include <iostream>
#include <thread>

Listener::Listener()
	: quit(false) {
	this->listen_cmd_thread(this->ListenCmd);
	this->listen_net_thread(this->ListenNet);
}

Listener::~Listener() {
	this->quit = true;
	this->listen_net_thread.join();
	this->listen_cmd_thread.join();
}

void Listener::ListenCmd() {
	std::string str_cmd;
	while(!this->quit) {
		std::cin >> str_cmd;
		// TODO: Parse string.
			// TODO: lock
			// TODO: Run command(command.id, command.data, command.length);
			// TODO: unlock
	}
}

void Listener::ListenNet() {
	UDPsocket socket = SDLNet_UDP_Open(DEFAULT_SERVER_PORT);
	if(!socket) {
		LogSdlError("SDLNet_UDP_Open");
		return;
	}

	UDPpacket* packet = SDLNet_AllocPacket(size); // TODO: unique_ptr. Find max length.
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
