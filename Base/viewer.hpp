#ifndef VIEWER_HPP
#define VIEWER_HPP

#include "base.hpp"
#include "viewer_sdl.hpp"

#include <vector>

class Viewer : public ViewerSdl {
private:
	void Clear();

	SDL_Texture* texture;
	std::vector<float> positions;
	void BeforDraw();
	void Draw();

	void Events();

public:
	Viewer();
	~Viewer();

	void Init();
	void DoStep(double seconds);
};

#endif // VIEWER_HPP
