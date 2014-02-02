#ifndef SDL_GL_VIEWER_HPP
#define SDL_GL_VIEWER_HPP

#include "base.hpp"
#include <string>

class SdlGlViewer {
protected:
	SDL_Window* window;
	SDL_Context* context;

	void Clear();

public:
	SdlGlViewer();
	~SdlGlViewer();

	bool Init(const std::string& title);

	void ClearViewer();
	void EndDraw();
};

#endif // SDL_GL_VIEWER_HPP
