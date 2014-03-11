#ifndef VIEWER_SDL_H
#define VIEWER_SDL_H

#include "_.hpp"

class ViewerSdl {
protected:
	SDL_Window* window;
	SDL_Renderer* renderer;

	void Clear();

public:
	ViewerSdl();
	~ViewerSdl();

	void Init(const std::string& title);

	SDL_Window* GetWindow();
	SDL_Renderer* GetRenderer() const;

	void ClearViewer();
	void EndDraw();

	SDL_Texture* CreateTexture(const std::string& filename) const;
	static void ReleaseTexture(SDL_Texture* texture);
	void DrawTexture(SDL_Texture* texture, int x = 0, int y = 0) const;
	void DrawRoundTexture(SDL_Texture* texture, int x, int y, int r, double a) const;

	SDL_Texture* CreateTextTexture(std::string text, std::string fontFile, SDL_Color color, int fontSize) const; // TODO: Correct functions to work with fonts.
};

#endif // VIEWER_SDL_H
