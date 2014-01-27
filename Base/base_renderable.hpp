#ifndef BASE_RENDERABLE_H
#define BASE_RENDERABLE_H

#include "base.hpp"
#include <string>

class BaseRenderable {
protected:
	SDL_Window* window;
	SDL_Renderer* renderer;

public:
	BaseRenderable();
	~BaseRenderable();

	void Clear();
	bool Init(const std::string& title);

	void ClearRenderer();
	void DrawRenderer();

	SDL_Texture* CreateTexture(const std::string& filename);
	void ReleaseTexture(SDL_Texture* texture);
	void DrawTexture(SDL_Texture* texture, int x = 0, int y = 0);

	void DrawContext();
};

#endif // BASE_RENDERABLE_H
