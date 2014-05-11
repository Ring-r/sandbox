#ifndef MAP_HPP
#define MAP_HPP

#include "_.hpp"
#include "entity.hpp"

class MapEntity {
public:
  static const uint8_t MIN_ROTATION = 0;
  static const uint8_t MAX_ROTATION = 3;

public:
  uint32_t position;
  uint32_t rotation;
  std::vector<uint32_t> commands;
  uint32_t position_next;
  bool can_move;
};

class Map {
public:
  static const uint32_t DEFAULT_COUNT_X = 100;
  static const uint32_t DEFAULT_COUNT_Y = 100;

  static const uint8_t DEFAULT_FILL_PERCENT = 10;

  static const uint8_t DEFAULT_CELL_SIZE = 32;

private:
	uint32_t count_x, count_y;
	std::vector<uint32_t> cells;

	uint32_t cell_size;
	std::vector<SDL_Texture*> textures;

	void Clear();

private:
  std::vector<MapEntity> entities;
  std::vector<uint32_t> cells_entities;

public:
	Map(uint32_t count_x = 0, uint32_t count_y = 0);
	~Map();

  void InitRandom(uint32_t count_x = DEFAULT_COUNT_X, uint32_t count_y = DEFAULT_COUNT_Y, uint8_t percent = DEFAULT_FILL_PERCENT);
	void Load(const ViewerSdl& viewer, const std::string& filename);
	void Draw(SDL_Renderer* renderer, const Entity& entity, float screen_center_x, float screen_center_y) const;

	void Updates(std::vector<Entity>& entity) const;

	void InitsRandom(std::vector<Entity>& entity) const;

public:
  void AddEntity(Entity& entity);
  void RemoveEntity(Entity& entity);

  void Update() {
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
	    it->rotation = it->rotation > MapEntity::MIN_ROTATION ? it->rotation - 1: MapEntity::MAX_ROTATION;
	    it->position_next = it->position;
	    break;
	  case 2: // Rotate clockwise.
	    it->rotation = it->rotation < MapEntity::MAX_ROTATION ? it->rotation + 1: MapEntity::MIN_ROTATION;
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
};

#endif // MAP_HPP
