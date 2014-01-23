#ifndef BASE_H
#define BASE_H

#include<SDL2/SDL.h>
#include <string>

class Base {
private:
	SDL_Window* window;
	SDL_Renderer* renderer;

public:
	Base();
	~Base();

	bool Init();

	SDL_Renderer* GetRenderer();

	SDL_Texture* CreateTexture(std::string filename);
	void ReleaseTexture(SDL_Texture* texture);
};

#endif // BASE_H
