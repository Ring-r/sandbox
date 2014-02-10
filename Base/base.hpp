#ifndef BASE_H
#define BASE_H

//#include <SDL2/SDL.h>
//#include <SDL/SDL_net.h>
#include <SDL.h>
#include <SDL_net.h>
#include <string>

#include <iostream>

class Base {
private:
	bool init;
	void Clear();

public:
	Base();
	~Base();

	void Init();
};

void LogSdlError(const std::string& msg);

#endif // BASE_H
