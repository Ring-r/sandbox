#include "map.hpp"

#include <random>

Map::Map()
	: count(0), positions(nullptr), vectors(nullptr) {
}

Map::~Map() {
	this->Clear();
}

bool Map::InitRandom(int count) {
	this->Clear();

	this->size[0] = Map::DEFAULT_SIZE; this->size[1] = Map::DEFAULT_SIZE; 

	this->count = count;

	std::random_device rand;
	int length = this->count << 1;
	this->positions = new float[length];
	this->vectors = new float[length];
	for(int i = 0, j = 0; i < length; ++i, j = (j + 1) % 2) {
		this->positions[i] = (this->size[j] - ENTITY_RADIUS) * rand() / RAND_MAX + ENTITY_RADIUS;
		this->vectors[i] = (MAX_SPEED_COEF - MIN_SPEED_COEF) * rand() / RAND_MAX + MIN_SPEED_COEF;
	}

	return true;
}

void Map::Clear() {
	this->size[0] = 0.0f; this->size[1] = 0.0f;
	if(this->vectors) {
		delete[] this->vectors;
	}
	if(this->positions) {
		delete[] this->positions;
	}
	this->count = 0;
}

void Map::Update() {
	int length = this->count << 1;
	for(int i = 0, j = 0; i < length; ++i, j = (j + 1) % 2) {
		this->positions[i] += this->vectors[i];
		if(this->positions[i] < 0) {
			this->vectors[i] = std::abs(this->vectors[i]);
		}
		if(this->positions[i] > this->size[j] - ENTITY_RADIUS) {
			this->vectors[i] = -std::abs(this->vectors[i]);
		}
	}
}
