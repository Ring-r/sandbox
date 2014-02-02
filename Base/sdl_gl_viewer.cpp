#include "sdl_gl_viewer.hpp"

SdlGlViewer::SdlGlViewer()
	: window(nullptr), context(nullptr) {
}

SdlGlViewer::~SdlGlViewer() {
	this->Clear();
}

void SdlGlViewer::Clear() {
	if(this->context) {
		SDL_DestroyContext(this->context);
		this->context = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
}

bool SdlGlViewer::Init(const std::string& title) {
	this->Clear();

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow(title.c_str(), SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, window_rect.w, window_rect.h, SDL_WINDOW_FULLSCREEN_DESKTOP | SDL_WINDOW_SHOWN);
	if(!this->window) {
		LogSdlError("SDL_CreateWindow");
		return false;
	}

	this->renderer = SDL_CreateContext(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->context) {
		LogSdlError("SDL_CreateRenderer");
		return false;
	}

	return true;
}

void SdlGlViewer::EndDraw() {
	SDL_GL_SwapWindow(this->window);
}
