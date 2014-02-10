#include "server.hpp"

#include "settings.hpp"

Server::Server()
	: Map() {
}

Server::~Server() {
}

void Server::Init(uint16_t port) {
	ListenerNet::Init(port);
}

void Server::Step() {
//	// TODO: Move code to thread or function?
//	UDPpacket packet;
//	packet.len = sizeof(float) / sizeof(Uint8) * (this->count << 1);
//	packet.data = new Uint8[packet.len];
//
//	while(!this->quit) {
//		this->Update();
//
//		// TODO: lock
//		memcpy(packet.data, this->positions, packet.len);
//		// TODO: unlock
//
//		SDLNet_ResolveHost(&packet.address, "127.0.0.1", DEFAULT_CLIENT_PORT);
//		UDPsocket socket = SDLNet_UDP_Open(DEFAULT_SERVER_PORT);
//		SDLNet_UDP_Send(socket, -1, &packet);
//		SDLNet_UDP_Close(socket);
//		// TODO: Send message to all connected client.
//
//		SDL_Delay(10);
//	}
//
//	delete packet.data;
}
