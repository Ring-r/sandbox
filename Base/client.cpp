#include "client.hpp"

Client::Client()
	: Listener(), BaseRenderable(), texture(nullptr), count(0), positions(nullptr) {
}

Client::~Client() {
	this->Clear();
}

bool Client::Init(int count) {
	BaseRenderable::Init("Client");

	this->Clear();

	this->texture = this->CreateTexture("./resources/entity.bmp");
	if(!this->texture) {
		return false;
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

//void Client::ListenNet() {
//	UDPsocket socket = SDLNet_UDP_Open(DEFAULT_CLIENT_PORT);
//	if(!socket) {
//		LogSdlError("SDLNet_UDP_Open");
//		return;
//	}
//
//	UDPpacket packet;
//	packet.len = sizeof(float) / sizeof(Uint8) * (this->count << 1);
//	packet.data = new Uint8[packet.len];
//
//	while (!this->quit) {
//		if(SDLNet_UDP_Recv(socket, &packet) > 0) {
//			// TODO: lock
//			memcpy(this->positions, packet.data, packet.len); // TODO: Clear before? Min...
//			// TODO: unlock
//		}
//	}
//
//	delete[] packet.data;
//	SDLNet_UDP_Close(socket);
//}

void Client::Run() {
	//while(!this->quit) {
	//	SDL_Event sdl_event;
	//	while(SDL_PollEvent(&sdl_event)) {
	//		if(sdl_event.type == SDL_QUIT) {
	//			this->quit = true;
	//		}
	//		if(sdl_event.type == SDL_KEYDOWN) {
	//			if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
	//				this->quit = true;
	//			}
	//		}
	//	}

	//	this->Update();
	//	this->ClearRenderer();
	//	this->Draw();
	//	this->DrawRenderer();

	//	// SDL_Delay(2000);
	//}
}
