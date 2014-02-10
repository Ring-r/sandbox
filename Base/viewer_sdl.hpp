#ifndef VIEWER_SDL_H
#define VIEWER_SDL_H

#include "base.hpp"
#include <string>

class ViewerSdl {
protected:
	SDL_Window* window;
	SDL_Renderer* renderer;

	void Clear();

public:
	ViewerSdl();
	~ViewerSdl();

	void Init(const std::string& title);

	void ClearViewer();
	void EndDraw();

	SDL_Texture* CreateTexture(const std::string& filename);
	void ReleaseTexture(SDL_Texture* texture);
	void DrawTexture(SDL_Texture* texture, int x = 0, int y = 0);

	// TODO: Add functions to work with fonts.
};

#endif // VIEWER_SDL_H
