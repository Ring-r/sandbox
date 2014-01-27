#include "server.hpp"

#include <random>
#include <thread>

Server::Server()
	: count(0), positions(nullptr), vectors(nullptr) {
}

Server::~Server() {
	this->Clear();
}

bool Server::Init(int count) {
	this->Clear();

	this->count = count;

	std::random_device rand;
	int size = this->count << 1;
	this->positions = new float[size];
	this->vectors = new float[size];
	for(int i = 0; i < size; ++i) {
		this->positions[i] = rand() % MAP_SIZE; // TODO: Use map.Width or other.
		this->vectors[i] = 1.0 * rand() / RAND_MAX;
	}

	return true;
}

void Server::Clear() {
	if(this->vectors) {
		delete[] this->vectors;
	}
	if(this->positions) {
		delete[] this->positions;
	}
	this->count = 0;
}

void Server::Update() {
	int size = this->count << 1;
	for(int i = 0; i < size; ++i) {
		this->positions[i] += this->vectors[i];
		if(this->positions[i] < 0) {
			this->vectors[i] = std::abs(this->vectors[i]);
		}
		if(this->positions[i] > MAP_SIZE - ENTITY_SIZE) {
			this->vectors[i] = -std::abs(this->vectors[i]);
		}
	}
}

void Server::ListenCmd(bool* quit) {
	std::string str_cmd;
	while(!*quit) {
		std::cin >> str_cmd;
		// TODO: Parse string.
			// TODO: lock
			// TODO: Run command(command.id, command.data, command.length);
			// TODO: unlock
	}
}

void Server::ListenNet(bool* quit) {
	UDPsocket socket = SDLNet_UDP_Open(DEFAULT_SERVER_PORT);
	if(!socket) {
		LogSdlError("SDLNet_UDP_Open");
		return;
	}

	UDPpacket* packet = SDLNet_AllocPacket(size); // TODO: unique_ptr. Find max length.
	while(!*quit) {
		if(SDLNet_UDP_Recv(socket, packet) > 0) {
			// TODO: lock
			// TODO: Run command(command.id, command.data, command.length);
			// TODO: unlock
		}
	}

	SDLNet_FreePacket(packet);
	SDLNet_UDP_Close(socket);
}

void Server::Run() {
	std::thread listen_cmd_thread(ListenCmd);
	std::thread listen_net_thread(ListenNet);

	// TODO: Move code to thread or function?
	UDPpacket packet;
	packet.len = sizeof(float) / sizeof(Uint8) * (this->count << 1);
	packet.data = new Uint8[packet.len];

	while(!this->quit) {
		this->Update();

		// TODO: lock
		memcpy(packet.data, this->positions, packet.len);
		// TODO: unlock

		SDLNet_ResolveHost(&packet.address, "127.0.0.1", DEFAULT_CLIENT_PORT);
		UDPsocket socket = SDLNet_UDP_Open(DEFAULT_SERVER_PORT);
		SDLNet_UDP_Send(socket, -1, &packet);
		SDLNet_UDP_Close(socket);
		// TODO: Send message to all connected client.

		SDL_Delay(10);
	}

	delete packet.data;

	listen_net_thread.join();
	listen_cmd_thread.join();
}
