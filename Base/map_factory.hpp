#ifndef MAP_FACTORY_HPP
#define MAP_FACTORY_HPP

#include "_.hpp"
#include "map.hpp"

class MapFactory {
public:
	static const uint32_t DEFAULT_COUNT_X = 100;
	static const uint32_t DEFAULT_COUNT_Y = 100;

	static const uint8_t DEFAULT_FILL_PERCENT = 10;

public:
	static void Inits(Map& map, const std::string& filename);
	static void InitsRandom(Map& map, uint32_t count_x, uint32_t count_y, uint8_t percent);
};

#endif // MAP_FACTORY_HPP
