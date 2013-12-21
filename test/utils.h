#include <iostream>

#include <SDL/SDL.h>

void PrintFps() {
	static int t0 = 0;
	static int frames = 0;
	++frames;

	int t = SDL_GetTicks();
	if (t - t0 >= 5000) {
		float seconds = (t - t0) / 1000.0;
		float fps = frames / seconds;
		std::cout << frames << " frames in " << seconds << " seconds = " << fps << " FPS.\n";
		t0 = t;
		frames = 0;
	}
}

float RandomFloat(float min, float max) {
	return static_cast<float>(rand()) / RAND_MAX * (max - min) + min;
}