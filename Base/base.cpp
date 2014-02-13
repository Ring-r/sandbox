#include "base.hpp"

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
	if(TTF_Init() < 0) {
		LogTtfError("TTF_Init");
		return;
	}
	this->init = true;
}

void LogError(const std::string& msg) {
	std::cerr << msg << " error: " << std::endl;
}

void LogSdlError(const std::string& msg) {
	std::cerr << msg << " error: " << SDL_GetError() << std::endl;
}

void LogTtfError(const std::string& msg) {
	std::cerr << msg << " error: " << TTF_GetError() << std::endl;
}
