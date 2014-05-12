#ifdef WIN32
	#include <SDL.h>
	#include <SDL_net.h>
	#include <SDL_ttf.h>
#elif LINUX
	#include <SDL2/SDL.h>
	#include <SDL2/SDL_net.h>
	#include <SDL2/SDL_ttf.h>
#endif
// TODO: #include <SDL2/SDL_console.h>

#include <algorithm>
#include <cstdint>
#include <cstdlib>
#include <fstream>
#include <iostream>
#include <map>
#include <cmath>
#include <string>
#include <vector>

#include "base.hpp"
#include "game.hpp"
#include "settings.hpp"

#include "viewer_sdl.hpp"
