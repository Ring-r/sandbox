#include "client.hpp"

#include "settings.hpp"

Client::Client()
	: ListenerNet(), SdlViewer(), texture(nullptr) {
}

Client::~Client() {
	this->Clear();
}

void Client::Clear() {
	if(this->texture) {
		this->ReleaseTexture(this->texture);
		this->texture = nullptr;
	}
}

void Client::SendToServer(uint32_t len, const uint8_t* data) {
	UDPpacket packet;
	packet.len = len;
	packet.data = const_cast<uint8_t*>(data);
	packet.address = this->server_ip_address;

	UDPsocket socket = SDLNet_UDP_Open(this->port);
	SDLNet_UDP_Send(socket, -1, &packet);
	SDLNet_UDP_Close(socket);
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

void Client::BeforDraw() {

}

void Client::Draw() {
	this->BeforDraw();
	this->ClearViewer();

	int size = this->positions.size() << 1;
	for(int i = 0; i < size; ++i) {
		this->DrawTexture(this->texture, this->positions[i], this->positions[i + 1]);
	}

	this->EndDraw();
}

void Client::Events() {
	uint32_t data(0);
	SDL_Event sdl_event;
	while(SDL_PollEvent(&sdl_event)) {
		if(sdl_event.type == SDL_QUIT) {
			this->quit = true;
		}
		if(sdl_event.type == SDL_KEYDOWN) {
			if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
					this->quit = true;
				}
			if(sdl_event.key.keysym.sym == SDLK_DOWN) {
					data |= 1;
				}
			if(sdl_event.key.keysym.sym == SDLK_LEFT) {
					data |= 2;
				}
			if(sdl_event.key.keysym.sym == SDLK_RIGHT) {
					data |= 4;
				}
			if(sdl_event.key.keysym.sym == SDLK_UP) {
					data |= 8;
				}
		}
	}
	uint32_t len = sizeof(uint32_t);
	data = data << sizeof(uint16_t) | 1;
	this->SendToServer(len, static_cast<uint8_t*>(&data));
}

void Client::Init(uint16_t port) {
	ListenerNet::Init(port);
	SdlViewer::Init("Client");

	this->Clear();
	this->texture = this->CreateTexture("./resources/entity.bmp");
}

void Client::DoStep(double seconds) {
	this->Events();
	this->Draw();
}

void Client::ConnectTo(const IPaddress& ip_address) {
	this->server_ip_address = ip_address;
	uint16_t id = 0;
	this->SendToServer(sizeof(uint16_t), static_cast<uint8_t*>(&id));
}
