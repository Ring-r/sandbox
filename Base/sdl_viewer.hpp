#ifndef SDL_VIEWER_H
#define SDL_VIEWER_H

#include "base.hpp"
#include <string>

class SdlViewer {
protected:
	SDL_Window* window;
	SDL_Renderer* renderer;

	void Clear();

public:
	SdlViewer();
	~SdlViewer();

	bool Init(const std::string& title);

	void ClearViewer();
	void EndDraw();

	SDL_Texture* CreateTexture(const std::string& filename);
	void ReleaseTexture(SDL_Texture* texture);
	void DrawTexture(SDL_Texture* texture, int x = 0, int y = 0);

	// TODO: Add function to work with fonts.
};

#endif // SDL_VIEWER_H
