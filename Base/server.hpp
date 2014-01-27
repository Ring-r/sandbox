#ifndef SERVER_H
#define SERVER_H

#include "base.hpp"

class Server {
private:
	static const ENTITY_SIZE = 64;
	static const MAP_SIZE = 700;
private:
	int count;
	float* positions;
	float* vectors;

	void Clear();
	void Update();

	bool quit;
	void Server::ListenCmd();
	void Server::ListenNet();

public:
	Server();
	~Server();

	bool Init(int count);
	void Run();
};

#endif // SERVER_H
