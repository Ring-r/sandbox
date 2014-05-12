#include "entity_viewer.hpp"

void EntityViewer::Draw(SDL_Renderer* renderer, uint8_t r, uint8_t g, uint8_t b) const {
	SDL_SetRenderDrawColor(renderer, r, g, b, 0);
	SDL_Rect rect; rect.x = this->px - this->r; rect.y = this->py - this->r; rect.w = rect.h = this->r + this->r;
	SDL_RenderFillRect(renderer, &rect);
}

void EntityViewer::Draw(SDL_Renderer* renderer, SDL_Texture* texture) const {
	SDL_Point point; point.x = this->r; point.y = this->r;
	SDL_Rect rect; rect.x = this->px - this->r; rect.y = this->py - this->r; rect.w = rect.h = this->r + this->r;
	SDL_RenderCopyEx(renderer, texture, NULL, &rect, this->angle, &point, SDL_FLIP_NONE);
}
