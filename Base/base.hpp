#ifndef BASE_H
#define BASE_H

#include<SDL2/SDL.h>
#include <string>

class Base {
protected:
	SDL_Window* window;
	SDL_GLContext gl_context;
	SDL_Renderer* renderer;

public:
	Base();
	~Base();

	bool Init(std::string title = "Base");

	void ClearRenderer();
	void DrawRenderer();

	SDL_Texture* CreateTexture(std::string filename);
	void ReleaseTexture(SDL_Texture* texture);
	void DrawTexture(SDL_Texture* texture, int x = 0, int y = 0);

	void DrawContext();
};

void LogSdlError(const std::string& msg);

#endif // BASE_H
