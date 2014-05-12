#ifndef MAP_HPP
#define MAP_HPP

#include "_.hpp"

class Map {
private:
	class Entity {
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

private:
	uint32_t count_x, count_y;
	std::vector<uint32_t> cells;

	std::vector<Entity> entities;
	std::vector<uint32_t> cells_entities;

	void Clear();

public:
	Map();

public:
	void AddEntity(uint32_t id);
	void RemoveEntity(uint32_t id);

	void Update();

	friend class MapFactory;
	friend class MapViewer;
};

#endif // MAP_HPP
