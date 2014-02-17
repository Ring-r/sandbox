#include "base.hpp"

void LogError(const std::string& msg) {
	std::cerr << msg << " error: " << std::endl;
}

void LogSdlError(const std::string& msg) {
	std::cerr << msg << " error: " << SDL_GetError() << std::endl;
}

void LogTtfError(const std::string& msg) {
	std::cerr << msg << " error: " << TTF_GetError() << std::endl;
}
