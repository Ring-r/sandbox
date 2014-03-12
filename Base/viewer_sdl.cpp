#include "viewer_sdl.hpp"

ViewerSdl::ViewerSdl()
	: window(nullptr), renderer(nullptr) {
}

ViewerSdl::~ViewerSdl() {
	this->Clear();
}

void ViewerSdl::Clear() {
	if(this->renderer) {
		SDL_DestroyRenderer(this->renderer);
		this->renderer = nullptr;
	}
	if(this->window) {
		SDL_DestroyWindow(this->window);
		this->window = nullptr;
	}
}

void ViewerSdl::Init(const std::string& title) {
	this->Clear();

	SDL_Rect window_rect;
	SDL_GetDisplayBounds(0, &window_rect);
	this->window = SDL_CreateWindow(title.c_str(), SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, window_rect.w >> 1, window_rect.h >> 1, SDL_WINDOW_RESIZABLE | SDL_WINDOW_SHOWN);
	if(!this->window) {
		LogSdlError("SDL_CreateWindow");
	}

	this->renderer = SDL_CreateRenderer(this->window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if(!this->renderer) {
		LogSdlError("SDL_CreateRenderer");
	}
}

SDL_Window* ViewerSdl::GetWindow() {
	return this->window;
}

SDL_Renderer* ViewerSdl::GetRenderer() const {
	return this->renderer;
}

void ViewerSdl::ClearViewer() {
	SDL_RenderClear(this->renderer);
}

void ViewerSdl::EndDraw() {
	SDL_RenderPresent(this->renderer);
}

SDL_Texture* ViewerSdl::CreateTexture(const std::string& filename) const {
	SDL_Surface* loadedImage = SDL_LoadBMP(filename.c_str());
	if(!loadedImage) {
		LogSdlError("SDL_LoadBMP");
		return nullptr;
	}

	SDL_Texture* texture = SDL_CreateTextureFromSurface(this->renderer, loadedImage);
	if(!texture) {
		LogSdlError("SDL_CreateTextureFromSurface");
	}
	SDL_FreeSurface(loadedImage);

	return texture;
}

void ViewerSdl::ReleaseTexture(SDL_Texture* texture) {
	if(texture) {
		SDL_DestroyTexture(texture);
	}
}

void ViewerSdl::DrawTexture(SDL_Texture* texture, int x, int y) const {
	SDL_Rect rect; rect.x = x; rect.y = y;
	SDL_QueryTexture(texture, NULL, NULL, &rect.w, &rect.h);
	SDL_RenderCopy(this->renderer, texture, NULL, &rect);
}

void ViewerSdl::DrawRoundTexture(SDL_Texture* texture, int x, int y, int r, double a) const {
	SDL_Point point; point.x = x; point.y = y;
	SDL_Rect rect; rect.x = x - r; rect.y = y - r;
	SDL_QueryTexture(texture, NULL, NULL, &rect.w, &rect.h);
	SDL_RenderCopyEx(this->renderer, texture, NULL, &rect, a, &point, SDL_FLIP_NONE);
}

SDL_Texture* ViewerSdl::CreateTextTexture(std::string text, std::string fontFile, SDL_Color color, int fontSize) const {
    TTF_Font *font = TTF_OpenFont(fontFile.c_str(), fontSize);
    if(!font) {
		LogTtfError("TTF_OpenFont");
		return nullptr;
	}
	
    SDL_Surface *surf = TTF_RenderText_Blended(font, text.c_str(), color);
    SDL_Texture *texture = SDL_CreateTextureFromSurface(renderer, surf);
    SDL_FreeSurface(surf);
    TTF_CloseFont(font);
 
    return texture;
}
