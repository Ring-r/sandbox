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

void collision(Entity& entity_i, Entity& entity_j)
{
	float vx = entity_j.px - entity_i.px;
    float vy = entity_j.py - entity_i.py;
    float d = std::sqrt(vx * vx + vy * vy);
    float r = entity_j.r + entity_i.r;

    if (EPS < d && d < r - EPS)
    {
        float coef = 0.5f * (r / d - 1);

        vx *= coef;
        vy *= coef;

        entity_j.px += vx;
        entity_j.py += vy;
        entity_i.px -= vx;
        entity_i.py -= vy;

        //entity_j.isCollide = true;
        //entity_i.isCollide = true;
    }
}
