#include "base_renderable.hpp"

BaseRenderable::BaseRenderable()
	: window(nullptr), renderer(nullptr) {
}

BaseRenderable::~BaseRenderable() {
	this->Clear();
}

void BaseRenderable::Clear() {
	if(this->renderer) {
		SDL_DestroyRenderer(this->renderer);
		this->renderer = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
}

bool BaseRenderable::Init(const std::string& title) {
	this->Clear();

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow(title.c_str(), SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, window_rect.w, window_rect.h, SDL_WINDOW_FULLSCREEN_DESKTOP | SDL_WINDOW_SHOWN);
	if(!this->window) {
		LogSdlError("SDL_CreateWindow");
		return false;
	}

	this->renderer = SDL_CreateRenderer(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->renderer) {
		LogSdlError("SDL_CreateRenderer");
		return false;
	}
	return true;
}

void BaseRenderable::ClearRenderer() {
	SDL_RenderClear(this->renderer);
}

void BaseRenderable::DrawRenderer() {
	SDL_RenderPresent(this->renderer);
}

SDL_Texture* BaseRenderable::CreateTexture(const std::string& filename) {
	SDL_Texture* texture = nullptr;
	SDL_Surface* loadedImage = SDL_LoadBMP(filename.c_str());
	if(!loadedImage) {
		LogSdlError("SDL_LoadBMP");
	}
	else {
		texture = SDL_CreateTextureFromSurface(this->renderer, loadedImage);
		if(!texture) {
			LogSdlError("SDL_CreateTextureFromSurface");
		}
		SDL_FreeSurface(loadedImage);
	}
	return texture;
}

void BaseRenderable::ReleaseTexture(SDL_Texture* texture) {
	if(texture) {
		SDL_DestroyTexture(texture);
	}
}

void BaseRenderable::DrawTexture(SDL_Texture* texture, int x, int y) {
	SDL_Rect rect;
	rect.x = x; rect.y = y;
	SDL_QueryTexture(texture, NULL, NULL, &rect.w, &rect.h);
	SDL_RenderCopy(this->renderer, texture, NULL, &rect);
}

void BaseRenderable::DrawContext() {
	SDL_GL_SwapWindow(this->window);
}
