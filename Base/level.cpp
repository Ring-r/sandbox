#include "level.hpp"

#include "entity_viewer.hpp"

Level::Level()
	: screen_center_x(0.0f), screen_center_y(0.0f), entity_index(0) {
}

Level::~Level() {
}

void Level::RandomEntityChange() {
	// TODO: Test code. Rewrite.
	const int moveProcent = 20;
	if(rand() % 100 < moveProcent) {
		int i = rand() % this->entities.size();
		if (i != this->entity_index) {
			Entity& entity = this->entities[i];
			if (rand() % 100 < 50){
				entity.angle = rand() % 360;
			}
			else
			{
				entity.speed = rand() % 4;
			}
			entity.vx = std::cos(entity.angle * TO_RAD) * entity.speed;
			entity.vy = std::sin(entity.angle * TO_RAD) * entity.speed;
		}
	}
}

void Level::DoStep() {
	if(entity_index < this->entities.size()) {
		this->entity_controller.entity = this->entities[entity_index];
	} // TODO: Dislike.
	this->entity_controller.DoStep();
	if(entity_index < this->entities.size()) {
		this->entities[entity_index] = this->entity_controller.entity;
	} // TODO: Dislike.

	for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
		it->DoStep();
	}
	this->collision_controller.Updates(this->entities);
	this->map.Updates(this->entities);

	this->RandomEntityChange();
}

void Level::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	if(renderer) {
		const Entity& entity = this->entity_controller.entity;
		this->map.Draw(renderer, entity, this->screen_center_x, this->screen_center_y);

		float angle_rad = entity.angle * TO_RAD;
		float sin_angle_rad = std::sin(angle_rad);
		float cos_angle_rad = std::cos(angle_rad);

		EntityViewer entity_viewer;
		for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
			entity_viewer.r = it->r;
			float x = it->px - entity.px;
			float y = it->py - entity.py;
			entity_viewer.px = this->screen_center_x + sin_angle_rad * x - cos_angle_rad * y;
			entity_viewer.py = this->screen_center_y + cos_angle_rad * x + sin_angle_rad * y;
			entity_viewer.angle = it->angle - entity.angle;
			entity_viewer.Draw(renderer, texture);
		}
	}
}

void Level::LoadMap(const ViewerSdl& viewer, const std::string& filename) {
	this->map.Load(viewer, filename);
	this->map.Updates(this->entities);
}

void Level::AddBots(uint8_t count, bool random_init) {
	this->entities.resize(this->entities.size() + count);
	if(random_init) {
		this->map.InitsRandom(this->entities);
		if(entity_index < this->entities.size()) {
			this->entity_controller.entity = this->entities[entity_index];
		}
	}
}

void Level::SetScreenCenter(float screen_center_x, float screen_center_y) {
	this->screen_center_x = screen_center_x;
	this->screen_center_y = screen_center_y;
}
