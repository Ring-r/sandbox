#include "collision_controller.hpp"

CollisionController::CollisionController()
	: count_x(DEFAULT_COUNT_X), count_y(DEFAULT_COUNT_Y), step_coef(DEFAULT_STEP_COEF) {

		this->cells.resize(this->count_x * this->count_y);
}

CollisionController::~CollisionController() {
}

void CollisionController::Updates(Entity& entity, uint32_t cell_index) {
	uint16_t x_index = cell_index % this->count_x;
	uint16_t y_index = cell_index / this->count_x;

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

	auto cell_it = this->cells.begin() + y_index * this->count_x + x_index;
	for(uint16_t ix = x_index; ix <= x_index_end; ++ix) {
		auto cell_it_temp = cell_it;
		for(uint16_t iy = y_index; iy <= y_index_end; ++iy) {
			for(auto it = cell_it->begin(); it < cell_it->end(); ++it) {
				if(&entity != *it) {
					collision(entity, **it);
				}
			}
			++cell_it;
		}
		cell_it = cell_it_temp + this->count_x; // TODO: Index out of range exception.
	}
}

void CollisionController::Updates(std::vector<Entity>& entities) {
	for(auto it_i = entities.begin(); it_i < entities.end() - 1; ++it_i) {
		for(auto it_j = it_i + 1; it_j < entities.end(); ++it_j) {
			collision(*it_i, *it_j);
		}
	}
}

void CollisionController::UpdatesOpt(std::vector<Entity>& entities) {
	for(auto it = this->cells.begin(); it < this->cells.end(); ++it) {
		it->clear();
	}
	this->indeces.clear();

	for(auto it = entities.begin(); it < entities.end(); ++it) {
		uint32_t x_index = static_cast<uint32_t>(it->px) >> this->step_coef;
		uint32_t y_index = static_cast<uint32_t>(it->py) >> this->step_coef;
		// TODO: if(0 <= x_index && x_index < this->count_x && 0 <= y_index && y_index < this->count_y)
		uint32_t index = y_index * this->count_x + x_index;
		this->cells[index].push_back(&(*it));
		this->indeces.push_back(index); // TODO: this->indeces.capacity = entities.size() if less.
	}

	auto it = entities.begin();
	auto iti = this->indeces.begin();
	for(; it < entities.end() && iti < this->indeces.end(); ++it, ++iti) {
		this->Updates(*it, *iti);
	}
}