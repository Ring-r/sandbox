#ifndef SETTINGS_HPP
#define SETTINGS_HPP

#include <cstdint>

class Settings {
public:
	uint16_t client_port;
	uint16_t server_port;

	Settings()
		: client_port(13013), server_port(13015) {
	}
};

#endif // SETTINGS_HPP
