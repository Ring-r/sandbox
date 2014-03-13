#ifndef TERMINAL_HPP
#define TERMINAL_HPP

#include "../_.hpp"
#include "../settings.hpp"

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
