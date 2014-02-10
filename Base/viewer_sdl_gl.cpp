#include "viewer_sdl_gl.hpp"

ViewerSdlGl::ViewerSdlGl()
	: window(nullptr), context(nullptr) {
}

ViewerSdlGl::~ViewerSdlGl() {
	this->Clear();
}

void ViewerSdlGl::Clear() {
	if(this->context) {
		SDL_DestroyContext(this->context);
		this->context = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
}

void ViewerSdlGl::Init(const std::string& title) {
	this->Clear();

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow(title.c_str(), SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, window_rect.w, window_rect.h, SDL_WINDOW_FULLSCREEN_DESKTOP | SDL_WINDOW_SHOWN);
	if(!this->window) {
		LogSdlError("SDL_CreateWindow");
	}

	this->renderer = SDL_CreateContext(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->context) {
		LogSdlError("SDL_CreateRenderer");
	}
}

void ViewerSdlGl::EndDraw() {
	SDL_GL_SwapWindow(this->window);
}
