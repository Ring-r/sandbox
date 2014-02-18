#include "level.hpp"

Level::Level(uint8_t count, uint8_t index, bool random_init)
	: size_x(DEFAULT_SIZE_X), size_y(DEFAULT_SIZE_Y) {
	this->heroes.resize(count);
	this->index = index;
	if(random_init) {
		for(int i = 0; i < this->heroes.size(); ++i) {
			this->heroes[i].angle = rand() % 360;
			this->heroes[i].px = 1.0f * rand() / RAND_MAX * this->size_x;
			this->heroes[i].py = 1.0f * rand() / RAND_MAX * this->size_y;
		}
	}
}

Level::~Level() {
}

void Level::DoStep() {
	for(int i = 0; i < this->heroes.size(); ++i) {
		if(i == this->index) {
			this->heroes[i].Hero::DoStep();
		}
		else {
			this->heroes[i].Entity::DoStep();
		}
		//if(this->xs[i] < 0 || this->size_x < this->xs[i] || this->ys[i] < 0 || this->size_y < this->ys[i]) {
		//	this->as[i] = rand() % 360;
		//	this->xs[i] = 1.0f * rand() / RAND_MAX * this->size_x;
		//	this->ys[i] = 1.0f * rand() / RAND_MAX * this->size_y;
		//}
	}
}

void Level::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	Hero hero;
	for(int i = 0; i < this->heroes.size(); ++i) {
		hero = this->heroes[i];
		float x = hero.px - this->heroes[this->index].px; // + 0.5f * this->size_x;
		float y = hero.py - this->heroes[this->index].py; // + 0.5f * this->size_y;
		float angle_rad = this->heroes[this->index].angle * TO_RAD;
		hero.px = std::sin(angle_rad) * x - std::cos(angle_rad) * y;
		hero.py = std::cos(angle_rad) * x + std::sin(angle_rad) * y;
		hero.px += 0.5f * this->size_x;
		hero.py += 0.5f * this->size_y;
		hero.angle = 0;
		hero.Draw(renderer, texture);
	}
}
