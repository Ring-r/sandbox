#include "map.hpp"

Map::Map()
  : count_x(0), count_y(0), cells(0), entities(0), cells_entities(0) {
}

void Map::AddEntity(uint32_t id) {
	// TODO:
}

void Map::RemoveEntity(uint32_t id) {
	 // TODO:
}

void Map::Update() {
	bool command_move;
	size_t command_index = 0;
	do {
		command_move = false;
		for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
			if(command_index < it->commands.size()) {
				command_move = true;
				auto command = it->commands[command_index];

				const size_t min_index = 0;
				const size_t max_index = this->cells_entities.size();
				switch(command) {
				case 0: // Move.
					switch(it->position) {
					case 0:
						it->position_next = it->position <= max_index - 1 ? it->position + 1 : it->position; // TODO: Check.
						break;
					case 1:
						it->position_next = it->position >= min_index + count_x ? it->position - count_x : it->position; // TODO: Check.
						break;
					case 2:
						it->position_next = it->position >= min_index + 1 ? it->position - 1 : it->position; // TODO: Check.
						break;
					case 3:
						it->position_next = it->position <= max_index - count_x ? it->position + count_x : it->position; // TODO: Check.
						break;
					}
					break;
				case 1: // Rotate counterclockwise.
					it->rotation = it->rotation > Map::Entity::MIN_ROTATION ? it->rotation - 1: Map::Entity::MAX_ROTATION;
					it->position_next = it->position;
					break;
				case 2: // Rotate clockwise.
					it->rotation = it->rotation < Map::Entity::MAX_ROTATION ? it->rotation + 1: Map::Entity::MIN_ROTATION;
					it->position_next = it->position;
					break;
				}

				it->can_move = it->position == it->position_next || !this->cells[it->position_next];
				if(it->can_move) {
					auto entity_next = this->cells_entities[it->position_next];
					if(entity_next) {
						it->can_move = false;
						this->entities[entity_next].can_move = false;
					}
				}
			}
			for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
				if(it->can_move) {
					it->position = it->position_next;
				}
			}
		}
		++command_index;
	} while(command_move);
}
