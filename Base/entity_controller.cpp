#include "entity_controller.hpp"

EntityController::EntityController() {
}

void EntityController::DoStep() {
	const uint8_t *keys = SDL_GetKeyboardState(NULL);
	float coef = keys[SDL_SCANCODE_DOWN] | keys[SDL_SCANCODE_UP]? 0.5f : 1.0f;
	this->entity.angle += coef * DEFAULT_ANGLE_STEP * (keys[SDL_SCANCODE_RIGHT] - keys[SDL_SCANCODE_LEFT]);
	this->entity.speed = DEFAULT_SPEED * (keys[SDL_SCANCODE_DOWN] - keys[SDL_SCANCODE_UP]);
	this->entity.vx = std::cos(this->entity.angle * TO_RAD) * this->entity.speed;
	this->entity.vy = std::sin(this->entity.angle * TO_RAD) * this->entity.speed;
	this->entity.px += this->entity.vx;
	this->entity.py += this->entity.vy;
}
