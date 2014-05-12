#include "map_factory.hpp"

void MapFactory::Inits(Map& map, const std::string& filename)
{
	std::ifstream input(filename);

	input >> map.count_x;
	input >> map.count_y;
	map.cells.resize(map.count_x * map.count_y);
	for(auto it = map.cells.begin(); it < map.cells.end(); ++it) {
		input >> *it;
	}
}

void MapFactory::InitsRandom(Map& map, uint32_t count_x, uint32_t count_y, uint8_t percent) {
  map.count_x = count_x;
  map.count_y = count_y;
  uint32_t count = count_x * count_y;
  uint32_t count_percent = count * percent / 100; // TODO: Magic number.
  map.cells.resize(count);
  fill(map.cells.begin(), map.cells.begin() + count_percent, 1);
  fill(map.cells.begin() + count_percent, map.cells.end(), 0);
  for(uint32_t i = 0; i < count; ++i) { // TODO: Find right value instead of count.
    std::swap(map.cells[rand() % count], map.cells[rand() % count]);
  }
}

//void MapFactory::Inits(const ViewerSdl& viewer, const std::string& filename)
//{
//	this->Clear();
//
//	std::ifstream input(filename);
//
//	input >> this->count_x;
//	input >> this->count_y;
//	this->cells.resize(this->count_x * this->count_y);
//	for(auto it = this->cells.begin(); it < this->cells.end(); ++it) {
//		input >> *it;
//	}
//
//	input >> this->cell_size;
//	uint32_t textures_count;
//	input >> textures_count;
//	std::string file_path;
//	for(uint32_t i = 0; i < textures_count; ++i) {
//		input >> file_path;
//		this->textures.push_back(viewer.CreateTexture(file_path));
//	}
//}
