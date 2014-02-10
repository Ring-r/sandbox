#ifndef VIEWER_SDL_GL_HPP
#define VIEWER_SDL_GL_HPP

#include "base.hpp"
#include <string>

class ViewerSdlGl {
protected:
	SDL_Window* window;
	SDL_Context* context;

	void Clear();

public:
	ViewerSdlGl();
	~ViewerSdlGl();

	void Init(const std::string& title);

	void ClearViewer();
	void EndDraw();
};

#endif // VIEWER_SDL_GL_HPP
