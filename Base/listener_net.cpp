#include "listener_net.hpp"

ListenerNet::ListenerNet()
	: init(false), port(0), socket(nullptr), packet(nullptr) {
}

ListenerNet::~ListenerNet() {
	this->Clear();
}

void ListenerNet::Clear() {
	this->init = false;
	if(this->packet) {
		SDLNet_FreePacket(this->packet);
		this->packet = nullptr;
	}
	if(this->socket) {
		SDLNet_UDP_Close(this->socket);
		this->socket = nullptr;
	}
	this->port = 0;
}

void ListenerNet::Init(uint16_t port) {
	this->Clear();

	this->port = port;
	this->socket = SDLNet_UDP_Open(this->port);
	if(!this->socket) {
		LogSdlError("SDLNet_UDP_Open");
		return;
	}
	this->packet = SDLNet_AllocPacket(MAX_PACKET_LEN);
	if(!this->packet) {
		LogSdlError("SDLNet_AllocPacket");
		return;
	}
	this->init = true;
}

void ListenerNet::DoStep() {
	if(this->init) {
		if(SDLNet_UDP_Recv(this->socket, this->packet) > 0) {
			// TODO: Check if this->packet.len > sizeof(uint16_t);
			// TODO: lock
			uint16_t command_id; // TODO: Copy this->packet to command_id.
			if(this->commands.find(command_id) != this->commands.end()) {
				(this->*(this->commands[command_id]))(this->packet); // TODO: Run command(command.id, command.data, command.length);
			}
			else {
				std::cout << ">> Unknown command." << std::endl;
			}
			// TODO: unlock
		}
	}

}
