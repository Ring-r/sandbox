#ifndef LISTENER_H
#define LISTENER_H

#include <map>
#include <string>
#include <thread>

class Listener {
protected:
	bool quit;
	std::map<std::string, void(Listener::*)()> commands;
	std::thread listen_net_thread;
	void ListenNet();
	void QuitCmd();

public:
	Listener(int port);
	~Listener();

	void Run();
};

const int MAX_COMMAND_LENGTH = 1024;

#endif // LISTENER_H
