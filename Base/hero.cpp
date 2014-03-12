#include "hero.hpp"

void Hero::DoStep() {
	const uint8_t *keys = SDL_GetKeyboardState(NULL);
	float coef = keys[SDL_SCANCODE_DOWN] | keys[SDL_SCANCODE_UP]? 0.5f : 1.0f;
	this->angle += coef * DEFAULT_ANGLE_STEP * (keys[SDL_SCANCODE_RIGHT] - keys[SDL_SCANCODE_LEFT]);
	this->speed = DEFAULT_SPEED * (keys[SDL_SCANCODE_DOWN] - keys[SDL_SCANCODE_UP]);
	this->vx = std::cos(this->angle * M_PI / 180) * this->speed;
	this->vy = std::sin(this->angle * M_PI / 180) * this->speed;
	this->px += this->vx;
	this->py += this->vy;
}
