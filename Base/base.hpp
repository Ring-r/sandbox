#ifndef BASE_H
#define BASE_H

#include<SDL.h>
//#include<SDL2/SDL.h>
#include <SDL_net.h>
//#include <SDL/SDL_net.h>
#include <string>

class Base {
private:
	int sdl_init_error;
	int sdlnet_init_error;

public:
	Base();
	~Base();
};

const int DEFAULT_SERVER_PORT = 11110;
const int DEFAULT_CLIENT_PORT = 11111;

void LogSdlError(const std::string& msg);

#endif // BASE_H
