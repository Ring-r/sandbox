#include "map_viewer.hpp"

#include <memory>

MapViewer::MapViewer()
	: cell_size(0), textures(0) {
}

MapViewer::~MapViewer() {
	for(auto it = this->textures.begin(); it < this->textures.end(); ++it) {
		ViewerSdl::ReleaseTexture(*it); // TODO: Check.
	}
	this->textures.clear();
}

void MapViewer::Draw(SDL_Renderer* renderer, const Map& map, uint32_t screen_size_x, uint32_t screen_size_y, float x, float y, float angle) const {
	if(renderer) {
		const uint8_t r = 0; // TODO: Add to resorces.
		const uint8_t g = 0; // TODO: Add to resorces.
		const uint8_t b = 255; // TODO: Add to resorces.
		const uint8_t a = 0; // TODO: Add to resorces.

		const float sin_a = sin(angle);
		const float cos_a = cos(angle);

		float px, py;

		auto it = map.cells.begin();
		py = 0;
		for (uint32_t iy = 0; iy < map.count_y; ++iy) {
			px = 0;
			for (uint32_t ix = 0; ix < map.count_x; ++ix) {
				if(*it) {
					SDL_SetRenderDrawColor(renderer, r, g, b, a);
					SDL_Rect rect;
					rect.x = (screen_size_x >> 1) + (int)( + (px - x) * cos_a + (py - y) * sin_a);
					rect.y = (screen_size_y >> 1) + (int)( - (px - x) * sin_a + (py - y) * cos_a);
					rect.w = rect.h = cell_size;
					SDL_RenderFillRect(renderer, &rect);
				}
				++it;
				px += cell_size;
			}
			py += cell_size;
		}
	}
}
