#include "map.hpp"

Map::Map()
	: count_x(DEFAULT_COUNT_X), count_y(DEFAULT_COUNT_Y), step_coef(DEFAULT_STEP_COEF) {

	this->cells.resize(this->count_x * this->count_y);
}

Map::~Map() {
}

void Map::Inits(Entity* entity) {
	uint8_t x_index = static_cast<uint8_t>(entity->px) >> this->step_coef;
	uint8_t y_index = static_cast<uint8_t>(entity->py) >> this->step_coef;
	// TODO: if(0 <= x_index && x_index < this->count_x && 0 <= y_index && y_index < this->count_y)
	entity->i = x_index + this->count_x * y_index;
	entity->j = static_cast<uint8_t>(this->cells[entity->i].size());
	this->cells[entity->i].push_back(entity);
}

void Map::Updates(Entity* entity) {
	std::vector<Entity*>& entities = this->cells[entity->i];
	std::iter_swap(entities.begin() + entity->j, entities.begin() + entities.size() - 1);
	entities[entity->j]->j = entity->j;
	entities.pop_back();

	this->Inits(entity);
}

void Map::CheckCollision(Entity* entity) {
	uint16_t x_index = entity->i % this->count_x;
	uint16_t y_index = entity->i / this->count_x;

	uint16_t step = 1; // TODO: Find right value.

	x_index = x_index > step ? x_index - step : 0;
	uint16_t x_index_end = x_index + step + step;
	if(x_index_end >= this->count_x) {
		x_index_end = this->count_x - 1;
	}

	y_index = y_index > step ? y_index - step : 0;
	uint16_t y_index_end = y_index + step + step;
	if(y_index_end >= this->count_y) {
		y_index_end = this->count_y - 1;
	}

	std::vector<Entity*>* entities = &(this->cells[y_index * this->count_x + x_index]);
	for(uint16_t ix = x_index; ix <= x_index_end; ++ix) {
		for(uint16_t iy = y_index; iy <= y_index_end; ++iy) {
			for(auto it = entities->begin(); it < entities->end(); ++it) {
				collision(*entity, **it);
			}
			++entities;
		}
		entities += this->count_x - x_index - 1; // TODO: Check.
	}
}