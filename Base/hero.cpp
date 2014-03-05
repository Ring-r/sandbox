#include "hero.hpp"

void Hero::DoStep() {
	const uint8_t *keys = SDL_GetKeyboardState(NULL);
	float coef = keys[SDL_SCANCODE_DOWN] | keys[SDL_SCANCODE_UP]? 0.5f : 1.0f;
	this->angle += coef * DEFAULT_ANGLE_STEP * (keys[SDL_SCANCODE_RIGHT] - keys[SDL_SCANCODE_LEFT]);
	this->speed = DEFAULT_SPEED * (keys[SDL_SCANCODE_DOWN] - keys[SDL_SCANCODE_UP]);
	this->vx = std::cos(this->angle * TO_RAD) * this->speed;
	this->vy = std::sin(this->angle * TO_RAD) * this->speed;
	this->px += this->vx;
	this->py += this->vy;
}

void Hero::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	SDL_Point point; point.x = this->r; point.y = this->r;
	SDL_Rect rect; rect.x = this->px - this->r; rect.y = this->py - this->r; rect.w = rect.h = this->r + this->r;
	SDL_RenderCopyEx(renderer, texture, NULL, &rect, this->angle, &point, SDL_FLIP_NONE);
}
