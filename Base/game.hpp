#ifndef GAME_HPP
#define GAME_HPP

#include "settings.hpp"

//#include <string>

class Game {
private:
	//Client client;
	//Server server;
	Settings settings;

	bool quit;
	void Listen();

	//void BaseInitClientCmd(const std::string& params);
	//void BaseInitServerCmd(const std::string& params);
	//void BaseQuitCmd(const std::string& params);
	//void ClientConnectToCmd(const std::string& params);
	//void ServerCreateMapCmd(const std::string& params);
	//void ServerPauseCmd(const std::string& params);
	//void ServerStartCmd(const std::string& params);
	//void ServerStopCmd(const std::string& params);

public:
	Game();
	~Game();

	void Run();
};

#endif // GAME_HPP
