#ifndef CLIENT_H
#define CLIENT_H

#include "base.hpp"
#include "listener.hpp"
#include "sdl_viewer.hpp"

class Client : public Listener, public SdlViewer {
private:
	SDL_Texture* texture;
	int count;
	float* positions;

	void Clear();
	void Update();
	void Draw();

public:
	Client();
	~Client();

	bool Init(int count);
	void Run();
};

#endif // CLIENT_H
