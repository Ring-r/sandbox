#include "viewer.hpp"

Viewer::Viewer()
	: ViewerSdl(), texture(nullptr) {
}

Viewer::~Viewer() {
	this->Clear();
}

void Viewer::Clear() {
	if(this->texture) {
		this->ReleaseTexture(this->texture);
		this->texture = nullptr;
	}
}

void Viewer::BeforDraw() {
	// TODO:
}

void Viewer::Draw() {
	this->BeforDraw();
	this->ClearViewer();

	int size = this->positions.size() << 1;
	for(int i = 0; i < size; ++i) {
		this->DrawTexture(this->texture, this->positions[i], this->positions[i + 1]);
	}

	this->EndDraw();
}

void Viewer::Events() {
	uint32_t data(0);
	SDL_Event sdl_event;
	while(SDL_PollEvent(&sdl_event)) {
		if(sdl_event.type == SDL_QUIT) {
			this->quit = true;
		}
		if(sdl_event.type == SDL_KEYDOWN) {
			if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
					this->quit = true;
				}
			//if(sdl_event.key.keysym.sym == SDLK_DOWN) {
			//if(sdl_event.key.keysym.sym == SDLK_LEFT) {
			//if(sdl_event.key.keysym.sym == SDLK_RIGHT) {
			//if(sdl_event.key.keysym.sym == SDLK_UP) {
		}
	}
}

void Viewer::DoStep(double seconds) {
	this->Events();
	this->Draw();
}
