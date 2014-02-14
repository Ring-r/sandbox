#ifndef SERVER_H
#define SERVER_H

#include "base.hpp"
#include "listener_net.hpp"
#include "map.hpp"

#include <cstdint>

class Server: public ListenerNet, public Map {
public:
	Server();
	~Server();

	void Init(uint16_t port);
	void Step();
};

#endif // SERVER_H
