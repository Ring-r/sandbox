#ifndef LISTENER_H
#define LISTENER_H

#include "base.hpp"
#include <thread>

class Listener {
protected:
	bool quit;
	std::thread listen_cmd_thread;
	std::thread listen_net_thread;
	void Server::ListenCmd();
	void Server::ListenNet();

public:
	Listener();
	~Listener();

	void Run();
};

#endif // LISTENER_H
