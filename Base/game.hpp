#ifndef GAME_HPP
#define GAME_HPP

#include "client.hpp"
#include "server.hpp"
#include "settings.hpp"

#include <map>
#include <string>
#include <thread>

class Game {
private:
	Client client;
	Server server;
	Settings settings;

	bool quit;
	std::map<std::string, void(Game::*)(const std::string& params)> commands;
	std::thread listen_thread;
	void Listen();

	void BaseInitClientCmd(const std::string& params);
	void BaseInitServerCmd(const std::string& params);
	void BaseQuitCmd(const std::string& params);
	void ClientConnectToCmd(const std::string& params);
	void ServerCreateMapCmd(const std::string& params);
	void ServerPauseCmd(const std::string& params);
	void ServerStartCmd(const std::string& params);
	void ServerStopCmd(const std::string& params);

public:
	Game();
	~Game();

	void Run();
};

const int TEMP_ENTITY_COUNT = 10;

#endif // GAME_HPP
