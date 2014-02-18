#include "entity.hpp"

Entity::Entity()
	: px(0.0f), vx(0.0f), py(0.0f), vy(0.0f), angle(0.0f), speed(DEFAULT_SPEED), r(DEFAULT_RADIUS) {
}

Entity::~Entity() {
}

void Entity::DoStep() {
	this->px += this->vx;
	this->py += this->vy;
}