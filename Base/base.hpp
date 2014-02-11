#ifndef BASE_H
#define BASE_H

#include <iostream>
#include <SDL.h> //#include <SDL2/SDL.h>
#include <SDL_net.h> //#include <SDL2/SDL_net.h>
#include <SDL_ttf.h> //#include <SDL2/SDL_ttf.h>
#include <string>

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
void LogTtfError(const std::string& msg);

#endif // BASE_H
