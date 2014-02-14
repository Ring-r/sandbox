#ifndef CLIENT_H
#define CLIENT_H

#include "base.hpp"
#include "listener_net.hpp"
#include "sdl_viewer.hpp"

#include <cstdint>

class Client : public ListenerNet, public SdlViewer {
private:
	IPaddress server_ip_address;
	void SendToServer(uint32_t len, const uint8_t* data);

	void Event();

public:
	Client();
	~Client();

	void Clear();

	//void Init(uint16_t port);
	void ConnectTo(const IPaddress& ip_address);

	void Init(SDL_Window* window, SDL_Renderer* renderer, uint16_t port);
	void Event(const SDL_Event& sdl_event);
	void DoStep();
};

#endif // CLIENT_H

//#include <vector>
//SDL_Texture* texture;
//std::vector<float> positions;
//void BeforDraw();
//void Draw();
