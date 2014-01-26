#include "client.hpp"

#include <SDL/SDL_net.h>
#include <thread>

#include <iostream>

Client::Client()
	: Base(), texture(nullptr), count(0), positions(nullptr), quit(false) {
}

Client::~Client() {
	this->Clear();
	SDLNet_Quit();
}

bool Client::Init(int count) {
	Base::Init();
	SDLNet_Init();

	this->Clear();

	this->texture = this->CreateTexture("./resources/entity.bmp");
	if(!this->texture) {
		return false; // TODO: Write to log and use default texture.
	}

	this->count = count;
	this->positions = new float[this->count << 1];
	std::fill_n(this->positions, this->count << 1, 0);

	return true;
}

void Client::Clear() {
	if(this->texture) {
		this->ReleaseTexture(this->texture);
		this->texture = nullptr;
	}
	if(this->positions) {
		delete[] this->positions;
		this->positions = nullptr;
	}
	this->count = 0;
}

void Client::Update() {

}

void Client::Draw() {
	int size = this->count << 1;
	for(int i = 0; i < size; ++i) {
		this->DrawTexture(this->texture, this->positions[i], this->positions[i + 1]);
	}
}

void GetMessages(float* data, int data_size, bool* quit) {
	UDPsocket socket = SDLNet_UDP_Open(DEFAULT_CLIENT_PORT);
	if(!socket) {
		std::cout << "SDLNet_UDP_Open" << std::endl; // TODO: Use log.
		return;
	}

	UDPpacket packet;
	packet.len = sizeof(float) / sizeof(Uint8) * (data_size << 1);
	packet.data = new Uint8[packet.len];

	std::cout << std::endl;
	while (!*quit) {
		if(SDLNet_UDP_Recv(socket, &packet) > 0) {
			// TODO: lock
			memcpy(data, packet.data, packet.len); // TODO: Clear before? Min...
			// TODO: unlock
		}
	}

	delete[] packet.data;
	SDLNet_UDP_Close(socket);
}

void Client::Run() {
	std::thread getMessages(GetMessages, this->positions, this->count << 1, &this->quit);

	this->quit = false;
	while(!this->quit) {
		SDL_Event sdl_event;
		while(SDL_PollEvent(&sdl_event)) {
			if(sdl_event.type == SDL_QUIT) {
				this->quit = true;
			}
			if(sdl_event.type == SDL_KEYDOWN) {
				if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
					this->quit = true;
				}
			}
		}

		this->Update();
		this->ClearRenderer();
		this->Draw();
		this->DrawRenderer();

		// SDL_Delay(2000);
	}
	getMessages.join();
}
