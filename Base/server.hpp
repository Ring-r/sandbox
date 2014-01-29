#ifndef SERVER_H
#define SERVER_H

#include "base.hpp"
#include "listener.hpp"
#include "map.hpp"

class Server: public Listener, public Map {
private:

public:
	Server();
	~Server();

	bool Init();
	void Run();
};

#endif // SERVER_H
