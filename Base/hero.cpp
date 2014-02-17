#include "hero.hpp"

Hero::Hero()
	: x(0), y(0), texture(nullptr) {
}

Hero::~Hero() {
	this->Clear();
}

void Hero::Clear() {
	if(this->texture) {
		ViewerSdl::ReleaseTexture(this->texture);
		this->texture = nullptr;
	}
	this->viewer = nullptr;
}

void Hero::Init(const ViewerSdl* viewer) {
	this->Clear();
	this->viewer = viewer;
	if(this->viewer) {
		this->texture = this->viewer->CreateTexture("./resources/entity.bmp");
	}
}

void Hero::Event(const SDL_Event& sdl_event) {
	const uint8_t *keys = SDL_GetKeyboardState(NULL);
	x += keys[SDL_SCANCODE_RIGHT] - keys[SDL_SCANCODE_LEFT];
	y += keys[SDL_SCANCODE_DOWN] - keys[SDL_SCANCODE_UP];
}

void Hero::DoStep() {
	if(this->viewer) {
		this->viewer->DrawTexture(this->texture, this->x, this->y);
	}
}
