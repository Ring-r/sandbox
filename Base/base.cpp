#include "base.hpp"

#include <iostream>

Base::Base()
	: sdl_init_error(-1), sdlnet_init_error(-1) {
	this->sdl_init_error = SDL_Init(SDL_INIT_EVERYTHING);
	if(this->sdl_init_error < 0) {
		LogSdlError("SDL_Init");
		return;
	}
	this->sdlnet_init_error = SDLNet_Init();
	if(this->sdlnet_init_error < 0) {
		LogSdlError("SDLNet_Init");
		return;
	}
}

Base::~Base() {
	if(!this->sdlnet_init_error) {
		SDLNet_Quit();
	}
	if(!this->sdl_init_error) {
		SDL_Quit();
	}
}

void LogSdlError(const std::string& msg) {
	std::cerr << msg << " error: " << SDL_GetError() << std::endl;
}
