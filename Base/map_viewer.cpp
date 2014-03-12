#include "map_viewer.h"

#include "entity_viewer.hpp"

MapViewer::MapViewer(uint32_t count_x, uint32_t count_y) {
	this->count_x = count_x;
	this->count_y = count_y;
	this->cells.resize(this->count_x * this->count_y);
}

MapViewer::~MapViewer() {
	this->Clear();
}

void MapViewer::Clear() {
	for(auto it = this->textures.begin(); it < this->textures.end(); ++it) {
		ViewerSdl::ReleaseTexture(*it); // TODO: Check.
	}
	this->count_x = 0;
	this->count_y = 0;
	this->cells.clear();
}

void MapViewer::LoadMap(const ViewerSdl& viewer, const std::string& filename)
{
	this->Clear();

	std::ifstream input(filename);
	uint32_t textures_count;
	input >> textures_count;
	std::string file_path;
	for(uint32_t i = 0; i < textures_count; ++i) {
		input >> file_path;
		this->textures.push_back(viewer.CreateTexture(file_path));
	}

	input >> this->count_x;
	input >> this->count_y;

	this->cells.resize(this->count_x * this->count_y);
	for(auto it = this->cells.begin(); it < this->cells.end(); ++it) {
		input >> *it;
	}
}

void MapViewer::DrawMap(SDL_Renderer* renderer, const Entity& entity, float screen_center_x, float screen_center_y) const {
	if(renderer) {
		EntityViewer block;
		block.angle = -entity.angle;
		block.r = DEFAULT_BLOCK_SIZE >> 1;

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
				py += DEFAULT_BLOCK_SIZE;
				}
			px += DEFAULT_BLOCK_SIZE;
			py = 0.0f;
			}
		}
}
