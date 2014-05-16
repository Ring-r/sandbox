#include "level.hpp"

#include "entity.hpp"
#include "map_factory.hpp"
#include "map_viewer.hpp"

Level::Level()
	: screen_center_x(0.0f), screen_center_y(0.0f) {
}

Level::~Level() {
}

void Level::DoStep() {
	//this->collision_controller.Updates(this->entities);
	//Updates(this->entities, 0.0f, 2 * this->screen_center_x, 0.0f, 2 * this->screen_center_y);
}

void Level::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	//Entity entity;
	//MapViewer mapViewer;
	//mapViewer.Draw(renderer, this->map, entity, this->screen_center_x, this->screen_center_y);
	// if(renderer) {
	// 	const Entity& entity = this->entity_controller.entity;
	// 	MapViewer mapViewer;
	// 	mapViewer.Draw(renderer, this->map, entity, this->screen_center_x, this->screen_center_y);

	//float angle_rad = entity.angle * TO_RAD;
	//float sin_angle_rad = std::sin(angle_rad);
	//float cos_angle_rad = std::cos(angle_rad);

	//EntityViewer entity_viewer;
	//for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
	//	entity_viewer.r = it->r;
	//	float x = it->px - entity.px;
	//	float y = it->py - entity.py;
	//	entity_viewer.px = this->screen_center_x + sin_angle_rad * x - cos_angle_rad * y;
	//	entity_viewer.py = this->screen_center_y + cos_angle_rad * x + sin_angle_rad * y;
	//	entity_viewer.angle = it->angle - entity.angle;
	//	entity_viewer.Draw(renderer, texture);
	//}
	// }
}

void Level::LoadMap(const ViewerSdl& viewer, const std::string& filename) {
	MapFactory::InitsRandom(this->map, MapFactory::DEFAULT_COUNT_X, MapFactory::DEFAULT_COUNT_Y, MapFactory::DEFAULT_FILL_PERCENT);
	//this->map.Updates(this->entities);
}


void Level::SetScreenCenter(float screen_center_x, float screen_center_y) {
	this->screen_center_x = screen_center_x;
	this->screen_center_y = screen_center_y;
}
