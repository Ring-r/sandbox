#include "base.h"

#include <iostream>

Base::Base()
	:window(nullptr), renderer(nullptr) {
}

Base::~Base() {
	if(this->renderer) {
		SDL_DestroyRenderer(this->renderer);
		this->renderer = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
	SDL_Quit();
}

bool Base::Init() {
	if(SDL_Init(SDL_INIT_EVERYTHING)) {
		std::cerr << "SDL_Init Error: " << SDL_GetError() << std::endl;
		return false;
	}

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow("Base", 0, 0, window_rect.w, window_rect.h, SDL_WINDOW_SHOWN);
	if(!this->window) {
		std::cerr << "SDL_CreateWindow Error: " << SDL_GetError() << std::endl;
		return false;
	}

	this->renderer = SDL_CreateRenderer(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->renderer) {
		std::cerr << "SDL_CreateRenderer Error: " << SDL_GetError() << std::endl;
		return false;
	}
	return true;
}

SDL_Renderer* Base::GetRenderer() {
	return this->renderer;
}

SDL_Texture* Base::CreateTexture(std::string filename) {
	SDL_Texture* texture = nullptr;
	SDL_Surface* loadedImage = SDL_LoadBMP(filename.c_str());
	if(loadedImage) {
		texture = SDL_CreateTextureFromSurface(this->renderer, loadedImage);
		if(!texture) {
			std::cerr << "SDL_CreateTextureFromSurface Error: " << SDL_GetError() << std::endl;
		}
		SDL_FreeSurface(loadedImage);
	}
	else {
		std::cerr << "SDL_LoadBMP Error: " << SDL_GetError() << std::endl;
	}
	return texture;
}

void Base::ReleaseTexture(SDL_Texture* texture) {
	if(texture) {
		SDL_DestroyTexture(texture);
	}
}
