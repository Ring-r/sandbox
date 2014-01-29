#ifndef LISTENER_H
#define LISTENER_H

#include <thread>

class Listener {
protected:
	bool quit;
	std::thread listen_cmd_thread;
	std::thread listen_net_thread;
	void ListenCmd();
	void ListenNet();

public:
	Listener();
	~Listener();
};

#endif // LISTENER_H
