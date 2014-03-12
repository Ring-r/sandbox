#include "level.hpp"

#include "entity_viewer.hpp"

Level::Level(uint8_t count, uint8_t index, bool random_init)
	: size_x(DEFAULT_SIZE_X), size_y(DEFAULT_SIZE_Y), screen_center_x(0.5f * DEFAULT_SIZE_X), screen_center_y(0.5f * DEFAULT_SIZE_Y) {

		this->heroes.resize(count);
		this->index = index;
		if(random_init) {
			for(auto it = this->heroes.begin(); it < this->heroes.end(); ++it) {
				it->angle = static_cast<float>(rand() % 360);
				it->px = 1.0f * rand() / RAND_MAX * this->size_x;
				it->py = 1.0f * rand() / RAND_MAX * this->size_y;
				this->map.Inits(static_cast<Entity*>(&it[0]));
			}
		}
}

Level::~Level() {
}

void Level::DoStep() {
	for(size_t i = 0; i < this->heroes.size(); ++i) {
		if(i == this->index) {
			this->heroes[i].Hero::DoStep();
		}
		else {
			this->heroes[i].Entity::DoStep();
		}

		//if(this->heroes[i].px < 0 || this->size_x < this->heroes[i].px || this->heroes[i].py < 0 || this->size_y < this->heroes[i].py) {
		//	this->heroes[i].angle = rand() % 360;
		//	this->heroes[i].px = 1.0f * rand() / RAND_MAX * this->size_x;
		//	this->heroes[i].py = 1.0f * rand() / RAND_MAX * this->size_y;
		//}

		this->map.Updates(&(this->heroes[i]));
	}

	//for(size_t i = 0; i < this->heroes.size() - 1; ++i) {
	//	for(size_t j = i + 1; j < this->heroes.size(); ++j) {
	//		collision(this->heroes[i], this->heroes[j]);
	//	}
	//}

	for(int i = 0; i < this->heroes.size(); ++i) {
		this->map.CheckCollision(&(this->heroes[i]));
	}

	for(int i = 0; i < this->heroes.size(); ++i) {
		this->map.Updates(&(this->heroes[i]));
	}
}

void Level::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	if(renderer) {
		const Hero& hero = this->heroes[this->index];
		this->map_viewer.DrawMap(renderer, hero, this->screen_center_x, this->screen_center_y);

		float angle_rad = hero.angle * TO_RAD;
		float sin_angle_rad = std::sin(angle_rad);
		float cos_angle_rad = std::cos(angle_rad);

		EntityViewer entity_viewer;
		for(auto it = this->heroes.begin(); it < this->heroes.end(); ++it) {
			entity_viewer.r = it->r;
			float x = it->px - hero.px;
			float y = it->py - hero.py;;
			entity_viewer.px = this->screen_center_x + sin_angle_rad * x - cos_angle_rad * y;
			entity_viewer.py = this->screen_center_x + cos_angle_rad * x + sin_angle_rad * y;
			entity_viewer.angle = it->angle - hero.angle;
			entity_viewer.Draw(renderer, texture);
		}
	}
}

void Level::LoadMap(const ViewerSdl& viewer, const std::string& filename) {
	this->map_viewer.LoadMap(viewer, filename);
}

void Level::SetScreenCenter(float screen_center_x, float screen_center_y) {
	this->screen_center_x = screen_center_x;
	this->screen_center_y = screen_center_y;
}
