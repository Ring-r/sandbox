#include "map.hpp"

#include <random>

Map::Map()
	: count(0), positions(nullptr), vectors(nullptr) {
}

Map::~Map() {
	this->Clear();
}
