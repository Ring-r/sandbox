#ifndef SETTINGS_HPP
#define SETTINGS_HPP

#include "_.hpp"

const std::string DEFAULT_TITLE = "Test";

const int DEFAULT_CLIENT_PORT = 11110;
const int DEFAULT_SERVER_PORT = 11111;

class Settings {
public:
	std::string title;
	uint16_t client_port;
	uint16_t server_port;

	Settings()
		: title(DEFAULT_TITLE), client_port(DEFAULT_CLIENT_PORT), server_port(DEFAULT_SERVER_PORT) {
	}
};

#endif // SETTINGS_HPP
