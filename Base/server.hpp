#ifndef SERVER_H
#define SERVER_H

class Server {
private:
	int count;
	float* positions; // TODO: uniqe_ptr?
	float* vectors; // TODO: uniqe_ptr?

	void Clear();
	void Update();

public:
	Server();
	~Server();

	bool Init(int count);
	void Run();
};

const int DEFAULT_SERVER_PORT = 11110;
const int DEFAULT_CLIENT_PORT = 11111; // TODO: Remove.

#endif // SERVER_H
