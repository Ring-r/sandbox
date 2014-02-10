#include "net_listener.hpp"

#include "base.hpp"
#include <algorithm>

NetListener::NetListener()
	: quit(false) {
	this->commands[0] = &NetListener::QuitCmd;
}

NetListener::~NetListener() {
	this->quit = true;
	if(this->listen_thread.joinable()) {
		this->listen_thread.join();
	}
}

void NetListener::Init(uint16_t port) {
	this->quit = true;
	if(this->listen_thread.joinable()) {
		this->listen_thread.join();
	}
	this->port = port;
	this->quit = false;
	this->listen_thread = std::thread(&NetListener::Listen, this);
}

void NetListener::Listen() {
	UDPsocket socket = SDLNet_UDP_Open(this->port);
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

void NetListener::QuitCmd() {
	this->quit = true;
}
