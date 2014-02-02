#include "game.hpp"

#include <algorithm>
#include <chrono>
#include <iostream>

Game::Game()
	: quit(false) {
	this->commands["quit"] = &Game::QuitCmd;
	this->commands["run_client"] = &Game::RunClientCmd;
	this->commands["run_server"] = &Game::RunServerCmd;
}

Game::~Game() {
	this->quit = true;
}

void Game::Run() {
	std::cout << ">> Run game. Enter command." << std::endl;
	std::string str;
	while(!this->quit) {
		std::cout << ": ";
		std::getline(std::cin, str);
		std::transform(str.begin(), str.end(), str.begin(), ::tolower);
		if(this->commands.find(str) != this->commands.end()) {
			(this->*(this->commands[str]))(); // TODO: Run command(command.id, command.data, command.length);
		}
		else {
			std::cout << ">> Unknown command." << std::endl;
		}
	}
}

void Game::QuitCmd() {
	this->quit = true;
	std::cout << ">> Quit game." << std::endl;
}

void Game::RunClientCmd() {
	if(!client.Init(TEMP_ENTITY_COUNT)) {
		std::cout << ">> Client not run." << std::endl;
		return;
	}
	//std::thread(&Client::Run, &client).detach();
	client.Run();
	std::cout << ">> Client run." << std::endl;
}

void Game::RunServerCmd() {
	if(!server.InitRandom(TEMP_ENTITY_COUNT)) {
		std::cout << ">> Server not run." << std::endl;
		return;
	}
	server.Run();
	std::cout << ">> Server run." << std::endl;
}
