#include "level.hpp"

Level::Level(uint8_t count, uint8_t index, bool random_init)
	: size_x(DEFAULT_SIZE_X), size_y(DEFAULT_SIZE_Y), screen_center_x(0.5f * DEFAULT_SIZE_X), screen_center_y(0.5f * DEFAULT_SIZE_Y) {
	//this->count_x = static_cast<uint16_t>(2 * this->size_x / DEFAULT_RADIUS) + 1;
	//this->count_y = static_cast<uint16_t>(2 * this->size_y / DEFAULT_RADIUS) + 1;
	//this->cells.resize(this->count_x * this->count_y);

	this->heroes.resize(count);
	this->index = index;
	if(random_init) {
		for(size_t i = 0; i < this->heroes.size(); ++i) {
			this->heroes[i].angle = rand() % 360;
			this->heroes[i].px = 1.0f * rand() / RAND_MAX * this->size_x;
			this->heroes[i].py = 1.0f * rand() / RAND_MAX * this->size_y;

			//this->heroes[i].cell_index = static_cast<uint16_t>(this->heroes[i].py / DEFAULT_CELL_SIZE) + this->count_y * static_cast<uint16_t>(this->heroes[i].px / DEFAULT_CELL_SIZE);
			//this->cells[this->heroes[i].cell_index] = i;
		}
	}
}

Level::~Level() {
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

		//this->cells[this->heroes[i].cell_index] = 1001;
		//this->heroes[i].cell_index = static_cast<uint16_t>(this->heroes[i].py / DEFAULT_CELL_SIZE) + this->count_y * static_cast<uint16_t>(this->heroes[i].px / DEFAULT_CELL_SIZE);
		//this->cells[this->heroes[i].cell_index] = i;
	}

	for(int i = 0; i < this->heroes.size() - 1; ++i) {
		for(int j = i + 1; j < this->heroes.size(); ++j) {
			collision(this->heroes[i], this->heroes[j]);
		}
	}

	//for(int i = 0; i < this->heroes.size(); ++i) {
	//	uint16_t index = this->heroes[i].cell_index;
	//	uint16_t index_x = this->heroes[i].cell_index / this->count_y;
	//	uint16_t index_y = this->heroes[i].cell_index % this->count_y;
	//	if(index_x != 0) {
	//		index_x -= 4;
	//	}
	//	uint16_t index_x_ = index_x + 8;
	//	if(index_x_ >= this->count_x) {
	//		index_x_ = this->count_x - 1;
	//	}

	//	if(index_y != 0) {
	//		index_y -= 4;
	//	}
	//	uint16_t index_y_ = index_y + 8;
	//	if(index_y_ >= this->count_y) {
	//		index_y_ = this->count_y - 1;
	//	}

	//	for(uint16_t ix = index_x; ix <= index_x_; ++ix) {
	//		for(uint16_t iy = index_y; iy <= index_y_; ++iy) {
	//			uint16_t index_j = iy + this->count_x * ix;
	//			if(this->cells[index_j] < 255) {
	//				collision(this->heroes[i], this->heroes[this->cells[index_j]]);
	//			}
	//		}
	//	}
	//}

	for(size_t i = 0; i < this->heroes.size() - 1; ++i) {
		for(size_t j = i + 1; j < this->heroes.size(); ++j) {
			collision(this->heroes[i], this->heroes[j]);
		}
	}
}

void Level::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	Hero hero;
	for(size_t i = 0; i < this->heroes.size(); ++i) {
		hero = this->heroes[i];
		float x = hero.px - this->heroes[this->index].px;
		float y = hero.py - this->heroes[this->index].py;
		float angle_rad = this->heroes[this->index].angle * TO_RAD;
		hero.px = std::sin(angle_rad) * x - std::cos(angle_rad) * y;
		hero.py = std::cos(angle_rad) * x + std::sin(angle_rad) * y;
		hero.px += this->screen_center_x;
		hero.py += this->screen_center_y;
		hero.angle -= this->heroes[this->index].angle;
		hero.Draw(renderer, texture);
	}
}

void Level::SetScreenCenter(float screen_center_x, float screen_center_y) {
	this->screen_center_x = screen_center_x;
	this->screen_center_y = screen_center_y;
}
