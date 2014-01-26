#ifndef CLIENT_H
#define CLIENT_H

#include "base.hpp"

class Client : public Base {
private:
	SDL_Texture* texture;
	int count;
	float* positions; // TODO: uniqe_ptr?
	bool quit;

	void Clear();
	void Update();
	void Draw();

public:
	Client();
	~Client();

	bool Init(int count);
	void Run();
};

const int DEFAULT_CLIENT_PORT = 11111;

#endif // CLIENT_H
