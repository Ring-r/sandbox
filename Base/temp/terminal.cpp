#include "terminal.hpp"

Terminal::Terminal()
	: init(false) {
}

Terminal::~Terminal() {
	this->Clear();
}

void Terminal::Clear() {
	this->init = false;

	for(int i = 0; i < this->textures.size(); ++i) {
		this->ReleaseTexture(this->textures[i]);
	}
	this->textures.clear();

	this->window = nullptr;
	this->renderer = nullptr;
}

void Terminal::Init(SDL_Window* window, SDL_Renderer* renderer) {
	this->Clear();

	if(!window) {
		LogError("viewer_sdl.window is nullptr");
		return;
	}
	this->window = window;
	if(!renderer) {
		LogError("viewer_sdl.renderer is nullptr");
		return;
	}
	this->renderer = renderer;

	SDL_Color color = { 192, 192, 192 };
	int fontSize = 14;

	int size = SDLK_z - SDLK_a + 1;
	for(int i = 0; i < size; ++i) {
		this->textures.push_back(this->CreateTextTexture(std::string(1, i + SDLK_a), "C:\\Windows\\Fonts\\couri.ttf", color, fontSize)); // TODO: Find cc0 ttf.
	}

	this->init = true;
}

void Terminal::Event(const SDL_Event& sdl_event) {
	if(sdl_event.type == SDL_KEYDOWN) {
		if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
			// TODO:
		}
		if(SDLK_a <= sdl_event.key.keysym.sym && sdl_event.key.keysym.sym <= SDLK_z) {
			this->str.push_back(sdl_event.key.keysym.sym - SDLK_a);
		}
		else if(sdl_event.key.keysym.sym == SDLK_BACKSPACE) {
			if(this->str.size() > 0) {
				this->str.erase(this->str.end() - 1);
			}
		}
	}
}

void Terminal::DoStep() {
	if(init) {
		int x = 0;
		int y = 100;
		for(int i = 0; i < this->str.size(); ++i) {
			this->DrawTexture(this->textures[str[i]], x, y);
			x += 10;
		}
	}
}