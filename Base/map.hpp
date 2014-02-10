#ifndef MAP_H
#define MAP_H

#include <vector>

class Map {
protected:
	float size[2]; // TODO: Magic number.

	int count;
	std::vector<float> positions;
	std::vector<float> vectors;

	void Clear();
	void Update();

public:
	Map();
	~Map();

	bool InitRandom(int count);
};

static const float DEFAULT_SIZE = 700.0f;
static const float ENTITY_RADIUS = 32.0f;
static const float MIN_SPEED_COEF = 0.5f;
static const float MAX_SPEED_COEF = 1.5f;

#endif // MAP_H
