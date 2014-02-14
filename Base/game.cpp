#include "game.hpp"

#include "client.hpp"
#include "server.hpp"

Game::Game()
	: quit(false) {
}

Game::~Game() {
	this->quit = true;
}

void Game::Event(const SDL_Event& sdl_event) {
	if(sdl_event.type == SDL_QUIT) {
		this->quit = true;
	}
	if(sdl_event.type == SDL_KEYDOWN) {
		if(sdl_event.key.keysym.sym == SDLK_ESCAPE) {
			this->quit = true;
		}
	}
}

void Game::Run() {
	this->quit = false;
	Client client; client.Init(this->window, this->renderer);
	while(!this->quit) {
		SDL_Event sdl_event;
		while(SDL_PollEvent(&sdl_event)) {
			this->Event(sdl_event);
			client.Event(sdl_event);
		}

		this->ClearViewer();
		client.DoStep();
		this->EndDraw();
	}
	client.Clear();
}

//void Game::Listen() {
	//std::cout << ">> Run game. Enter command." << std::endl;
	//std::string str;
	//while(!this->quit) {
	//	std::cout << ": ";
	//	std::getline(std::cin, str);
	//	std::transform(str.begin(), str.end(), str.begin(), ::tolower);
	//	if(this->commands.find(str) != this->commands.end()) {
	//		(this->*(this->commands[str]))(str); // TODO: Run command(command.id, command.data, command.length);
	//	}
	//	else {
	//		std::cout << ">> Unknown command." << std::endl;
	//	}
	//}
//}

//// Common commands:
//this->commands["init_client"] = &Game::BaseInitClientCmd;
//this->commands["init_server"] = &Game::BaseInitServerCmd;
//this->commands["quit"] = &Game::BaseQuitCmd;
//// Client commands:
//this->commands["connect_to"] = &Game::ClientConnectToCmd;
//// Server commands:
//this->commands["create_map"] = &Game::ServerCreateMapCmd;
//this->commands["pause"] = &Game::ServerPauseCmd;
//this->commands["start"] = &Game::ServerStartCmd;
//this->commands["stop"] = &Game::ServerStopCmd;


//void Game::BaseInitClientCmd(const std::string& params) {
//	// TODO: Parse params.
//	uint16_t port = this->settings.client_port;
//	this->client.Init(port);
//	std::cout << ">> End init_client." << std::endl;
//}
//
//void Game::BaseInitServerCmd(const std::string& params) {
//	// TODO: Parse params.
//	uint16_t port = this->settings.server_port;
//	this->server.Init(port);
//	std::cout << ">> End init_server." << std::endl;
//}
//
//void Game::BaseQuitCmd(const std::string& params) {
//	this->quit = true;
//	std::cout << ">> End quit." << std::endl;
//}
//
//void Game::ClientConnectToCmd(const std::string& params) {
//	// TODO: Parse params.
//	std::string host = "127.0.0.1";
//	uint16_t port = this->settings.server_port;
//	IPaddress ip_address;
//	SDLNet_ResolveHost(&ip_address, host.c_str(), port);
//	this->client.ConnectTo(ip_address);
//	std::cout << ">> End client_connect_to." << std::endl;
//}
//
//void Game::ServerCreateMapCmd(const std::string& params) {
//	std::cout << ">> End server_create_map." << std::endl;
//}
//
//void Game::ServerPauseCmd(const std::string& params) {
//	std::cout << ">> End server_pause." << std::endl;
//}
//
//void Game::ServerStartCmd(const std::string& params) {
//	std::cout << ">> End server_start." << std::endl;
//}
//
//
//void Game::ServerStopCmd(const std::string& params) {
//	std::cout << ">> End server_stop." << std::endl;
//}
