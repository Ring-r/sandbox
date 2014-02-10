#include "base.hpp"

#include <iostream>

Base::Base()
	: init(false) {
}

Base::~Base() {
	this->Clear();
}

void Base::Clear() {
	this->init = false;
	SDLNet_Quit();
	SDL_Quit();
}

void Base::Init() {
	this->Clear();

	if(SDL_Init(SDL_INIT_EVERYTHING) < 0) {
		LogSdlError("SDL_Init");
		return;
	}
	if(SDLNet_Init() < 0) {
		LogSdlError("SDLNet_Init");
		return;
	}
	this->init = true;
}

void LogSdlError(const std::string& msg) {
	std::cerr << msg << " error: " << SDL_GetError() << std::endl;
}
