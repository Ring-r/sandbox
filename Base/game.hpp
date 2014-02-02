#ifndef GAME_HPP
#define GAME_HPP

#include "client.hpp"
#include "server.hpp"

#include <map>
#include <string>

class Game {
private:
	bool quit;
	Client client;
	Server server;
	std::map<std::string, void(Game::*)()> commands;

	void QuitCmd();
	void RunClientCmd();
	void RunServerCmd();

public:
	Game();
	~Game();

	void Run();
};

const int TEMP_ENTITY_COUNT = 10;

#endif // GAME_HPP
