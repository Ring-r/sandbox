#include "map.hpp"

#include "entity_viewer.hpp"

Map::Map(uint32_t count_x, uint32_t count_y)
  : count_x(count_x), count_y(count_y), cells(count_x * count_y, 0) {
}

Map::~Map() {
	this->Clear();
}

void Map::Clear() {
	for(auto it = this->textures.begin(); it < this->textures.end(); ++it) {
		ViewerSdl::ReleaseTexture(*it); // TODO: Check.
	}
	this->count_x = 0;
	this->count_y = 0;
	this->cells.clear();
}

void Map::InitRandom(uint32_t count_x, uint32_t count_y, uint8_t percent) {
  this->count_x = count_x;
  this->count_y = count_y;
  uint32_t count = count_x * count_y;
  uint32_t count_percent = count * percent / 100; // TODO: Magic number.
  this->cells.resize(count);
  fill(this->cells.begin(), this->cells.begin() + count_percent, 1);
  fill(this->cells.begin() + count_percent, this->cells.end(), 0);
  for(uint32_t i; i < count; ++i) { // TODO: Find right value instead of count.
    std::swap(this->cells[rand() % count], this->cells[rand() % count]);
  }
}

void Map::Load(const ViewerSdl& viewer, const std::string& filename)
{
	this->Clear();

	std::ifstream input(filename);

	input >> this->count_x;
	input >> this->count_y;
	this->cells.resize(this->count_x * this->count_y);
	for(auto it = this->cells.begin(); it < this->cells.end(); ++it) {
		input >> *it;
	}

	input >> this->cell_size;
	uint32_t textures_count;
	input >> textures_count;
	std::string file_path;
	for(uint32_t i = 0; i < textures_count; ++i) {
		input >> file_path;
		this->textures.push_back(viewer.CreateTexture(file_path));
	}
}

void Map::Draw(SDL_Renderer* renderer, const Entity& entity, float screen_center_x, float screen_center_y) const {
	if(renderer) {
		EntityViewer block;
		block.angle = -entity.angle;
		block.r = DEFAULT_CELL_SIZE >> 1;

		float angle_rad = entity.angle * TO_RAD;
		float sin_angle_rad = std::sin(angle_rad);
		float cos_angle_rad = std::cos(angle_rad);

		float px = 0.0f;
		float py = 0.0f;
		auto it = this->cells.begin();
		for (uint32_t i = 0; i < this->count_x; ++i) {
			for (uint32_t j = 0; j < this->count_y; ++j) {
				if(*it) {
					float x = px - entity.px;
					float y = py - entity.py;
					block.px = screen_center_x + sin_angle_rad * x - cos_angle_rad * y;
					block.py = screen_center_y + cos_angle_rad * x + sin_angle_rad * y;
					block.Draw(renderer, this->textures[*it - 1]);
				}
				++it;
				py += DEFAULT_CELL_SIZE;
			}
			px += DEFAULT_CELL_SIZE;
			py = 0.0f;
		}
	}
}

void Map::Updates(std::vector<Entity>& entities) const{
  uint32_t size_x = this->count_x * this->cell_size;
  uint32_t size_y = this->count_y * this->cell_size;
	for(auto it = entities.begin(); it < entities.end(); ++it) {
		it->px = std::max(it->px, it->r); it->px = std::min(it->px, size_x - it->r);
		it->py = std::max(it->py, it->r); it->py = std::min(it->py, size_y - it->r);
	}
}

void Map::InitsRandom(std::vector<Entity>& entities) const{
  uint32_t size_x = this->count_x * this->cell_size;
  uint32_t size_y = this->count_y * this->cell_size;
	for(auto it = entities.begin(); it < entities.end(); ++it) {
		it->angle = static_cast<float>(rand() % 360);
		it->px = 1.0f * rand() / RAND_MAX * size_x;
		it->py = 1.0f * rand() / RAND_MAX * size_y;
	}
}
