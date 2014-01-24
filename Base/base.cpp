#include "base.h"

#include <iostream>

Base::Base()
	: window(nullptr), gl_context(nullptr), renderer(nullptr) {
}

Base::~Base() {
	if(this->renderer) {
		SDL_DestroyRenderer(this->renderer);
		this->renderer = nullptr;
	}
	if(this->gl_context) {
		SDL_GL_DeleteContext(gl_context);
		this->gl_context = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
	SDL_Quit();
}

bool Base::Init(std::string title) {
	if(SDL_Init(SDL_INIT_EVERYTHING) < 0) {
		LogSdlError("SDL_Init");
		return false;
	}

    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 3);
    SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 2);

    SDL_GL_SetAttribute(SDL_GL_DEPTH_SIZE, 24);

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow(title.c_str(), SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, window_rect.w, window_rect.h, SDL_WINDOW_OPENGL | SDL_WINDOW_FULLSCREEN_DESKTOP | SDL_WINDOW_SHOWN);
	if(!this->window) {
		LogSdlError("SDL_CreateWindow");
		return false;
	}

	this->gl_context = SDL_GL_CreateContext(this->window);
	if(!this->gl_context) {
		LogSdlError("SDL_GL_CreateContext");
		return false;
	}

	this->renderer = SDL_CreateRenderer(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->renderer) {
		LogSdlError("SDL_CreateRenderer");
		return false;
	}
	return true;
}

void Base::ClearRenderer() {
	SDL_RenderClear(this->renderer);
}

void Base::DrawRenderer() {
	SDL_RenderPresent(this->renderer);
}

SDL_Texture* Base::CreateTexture(std::string filename) {
	SDL_Texture* texture = nullptr;
	SDL_Surface* loadedImage = SDL_LoadBMP(filename.c_str());
	if(loadedImage) {
		texture = SDL_CreateTextureFromSurface(this->renderer, loadedImage);
		if(!texture) {
			LogSdlError("SDL_CreateTextureFromSurface");
		}
		SDL_FreeSurface(loadedImage);
	}
	else {
		LogSdlError("SDL_LoadBMP");
	}
	return texture;
}

void Base::ReleaseTexture(SDL_Texture* texture) {
	if(texture) {
		SDL_DestroyTexture(texture);
	}
}

void Base::DrawTexture(SDL_Texture* texture, int x, int y) {
	SDL_Rect rect;
	rect.x = x; rect.y = y;
	SDL_QueryTexture(texture, NULL, NULL, &rect.w, &rect.h);
	SDL_RenderCopy(this->renderer, texture, NULL, &rect);
}

void Base::DrawContext() {
	SDL_GL_SwapWindow(this->window);
}

void LogSdlError(const std::string& msg) {
	std::cerr << msg << " error: " << SDL_GetError() << std::endl;
}
