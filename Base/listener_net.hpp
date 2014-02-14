#ifndef LISTENER_NET_H
#define LISTENER_NET_H

#include "base.hpp"

#include <cstdint>
#include <map>

abstract class ListenerNet {
protected:
	bool init;
	uint16_t port;
	UDPsocket socket;
	UDPpacket* packet;
	std::map<uint16_t, void(ListenerNet::*)()> commands;

	void Clear();

public:
	ListenerNet();
	~ListenerNet();

	void Init(uint16_t port);
	void DoStep();
};

const int MAX_PACKET_LEN = 1024; // TODO: Remove this.

#endif // LISTENER_NET_H
