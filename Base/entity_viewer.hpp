#ifndef ENTITY_VIEWER_HPP
#define ENTITY_VIEWER_HPP

#include "entity.hpp"

class EntityViewer : public Entity {
public:
	void Draw(SDL_Renderer* renderer, SDL_Texture* texture) const;
};

#endif // ENTITY_VIEWER_HPP
