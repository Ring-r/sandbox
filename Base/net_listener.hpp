#ifndef NET_LISTENER_H
#define NET_LISTENER_H

#include <cstdint>
#include <map>
#include <string>
#include <thread>

class NetListener {
protected:
	bool quit;
	std::map<uint16_t, void(NetListener::*)()> commands;
	std::thread listen_thread;
	uint16_t port;

	void Listen();
	void QuitCmd();

public:
	NetListener();
	~NetListener();

	void Init(uint16_t port);
};

const int MAX_COMMAND_LENGTH = 1024;

#endif // NET_LISTENER_H
