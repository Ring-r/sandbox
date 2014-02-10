#include "listener_cmd.hpp"

#include <algorithm>

ListenerCmd::ListenerCmd()
	: init(false), buffer() {
}

ListenerCmd::~ListenerCmd() {
	this->Clear();
}

void ListenerCmd::Clear() {
	this->init = false;
	this->buffer.clear();
}

void ListenerCmd::Init() {
	this->Clear();
	this->init = true;
}

void ListenerCmd::DoStep() {
	//if(this->init) {
	//	if(SDLNet_UDP_Recv(this->socket, this->packet) > 0) {
	//		// TODO: Check if this->packet.len > sizeof(uint16_t);
	//		// TODO: lock
	//		uint16_t command_id; // TODO: Copy this->packet to command_id.
	//		if(this->commands.find(command_id) != this->commands.end()) {
	//			(this->*(this->commands[command_id]))(this->packet); // TODO: Run command(command.id, command.data, command.length);
	//		}
	//		else {
	//			std::cout << ">> Unknown command." << std::endl;
	//		}
	//		// TODO: unlock
	//	}
}
