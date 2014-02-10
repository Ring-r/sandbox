#ifndef SETTINGS_HPP
#define SETTINGS_HPP

#include <cstdint>

const int DEFAULT_CLIENT_PORT = 11110;
const int DEFAULT_SERVER_PORT = 11111;

class Settings {
public:
	uint16_t client_port;
	uint16_t server_port;

	Settings()
		: client_port(DEFAULT_CLIENT_PORT), server_port(DEFAULT_SERVER_PORT) {
	}
};

#endif // SETTINGS_HPP
