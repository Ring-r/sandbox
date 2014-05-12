#include "map_viewer.hpp"

#include "entity_viewer.hpp"

MapViewer::MapViewer()
  : textures(0) {
}

MapViewer::~MapViewer() {
	for(auto it = this->textures.begin(); it < this->textures.end(); ++it) {
		ViewerSdl::ReleaseTexture(*it); // TODO: Check.
	}
	this->textures.clear();
}

//void MapViewer::Draw(SDL_Renderer* renderer, const Map& map, const Entity& entity, float screen_center_x, float screen_center_y) const {
//	if(renderer) {
//		EntityViewer block;
//		block.angle = -entity.angle;
//		block.r = DEFAULT_CELL_SIZE >> 1;
//
//		float angle_rad = entity.angle * TO_RAD;
//		float sin_angle_rad = std::sin(angle_rad);
//		float cos_angle_rad = std::cos(angle_rad);
//
//		float px = 0.0f;
//		float py = 0.0f;
//		auto it = map.cells.begin();
//		for (uint32_t i = 0; i < map.count_x; ++i) {
//			for (uint32_t j = 0; j < map.count_y; ++j) {
//				if(*it) {
//					float x = px - entity.px;
//					float y = py - entity.py;
//					block.px = screen_center_x + sin_angle_rad * x - cos_angle_rad * y;
//					block.py = screen_center_y + cos_angle_rad * x + sin_angle_rad * y;
//					if(*it - 1 < this->textures.size()) {
//						block.Draw(renderer, 0, 0, 255);
//					}
//					else {
//						block.Draw(renderer, this->textures[*it - 1]);
//					}
//				}
//				++it;
//				py += DEFAULT_CELL_SIZE;
//			}
//			px += DEFAULT_CELL_SIZE;
//			py = 0.0f;
//		}
//	}
//}

void MapViewer::Draw(SDL_Renderer* renderer, const Map& map, const Entity& entity, float screen_center_x, float screen_center_y) const {
	if(renderer) {
		EntityViewer block;
		block.angle = -entity.angle;
		block.r = DEFAULT_CELL_SIZE >> 1;

		float px = 0.0f;
		float py = 0.0f;
		auto it = map.cells.begin();
		for (uint32_t i = 0; i < map.count_x; ++i) {
			for (uint32_t j = 0; j < map.count_y; ++j) {
				if(*it) {
					float x = px - entity.px;
					float y = py - entity.py;
					block.px = screen_center_x + x;
					block.py = screen_center_y + y;
					if(*it - 1 >= this->textures.size()) {
						block.Draw(renderer, 0, 0, 255);
					}
					else {
						block.Draw(renderer, this->textures[*it]);
					}
				}
				++it;
				py += DEFAULT_CELL_SIZE;
			}
			px += DEFAULT_CELL_SIZE;
			py = 0.0f;
		}
	}
}
