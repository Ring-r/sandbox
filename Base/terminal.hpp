#ifndef TERMINAL_HPP
#define TERMINAL_HPP

#include "base.hpp"
#include "viewer_sdl.hpp"
#include "settings.hpp"

#include <string>
#include <vector>

class Terminal : public ViewerSdl {
private:
	bool init;
	std::string str;
	std::vector<SDL_Texture*> textures; // TODO: SDL_Texture* texture;

public:
	Terminal();
	~Terminal();

	void Clear();
	void Init(SDL_Window* window, SDL_Renderer* renderer);
	void Event(const SDL_Event& sdl_event);
	void DoStep();
};

#endif // TERMINAL_HPP
