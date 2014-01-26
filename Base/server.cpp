#include "server.hpp"

#include <SDL2/SDL.h>
#include <SDL/SDL_net.h>
#include <random>
#include "base.hpp"

Server::Server()
	: count(0), positions(nullptr), vectors(nullptr) {
}

Server::~Server() {
	this->Clear();
	SDLNet_Quit();
	SDL_Quit();
}

bool Server::Init(int count) {
	if(SDL_Init(SDL_INIT_EVERYTHING) < 0) {
		// LogSdlError("SDL_Init"); // TODO:
		return false;
	}
	SDLNet_Init();

	this->Clear();

	this->count = count;

	std::random_device rand;
	int size = this->count << 1;
	this->positions = new float[size];
	this->vectors = new float[size];
	for(int i = 0; i < size; ++i) {
		this->positions[i] = rand() % 700; // TODO: Use map.Width or other.
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
		if(this->positions[i] > 700 - 64) {
			this->vectors[i] = -std::abs(this->vectors[i]);
		}
	}
}

void Server::Run() {
	// TODO: Create thread and get all message.

	UDPpacket packet;
	packet.len = sizeof(float) / sizeof(Uint8) * (this->count << 1);
	packet.data = new Uint8[packet.len];

	for(int i = 0; i < 5000; ++i) {
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
}
