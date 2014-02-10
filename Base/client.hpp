#ifndef CLIENT_H
#define CLIENT_H

#include "base.hpp"
#include "net_listener.hpp"
#include "sdl_viewer.hpp"

#include <cstdint>
#include <thread>
#include <vector>

class Client : public ListenerNet, public SdlViewer {
private:
	void Clear();

	IPaddress server_ip_address;
	void SendToServer(uint32_t len, const uint8_t* data);

	SDL_Texture* texture;
	std::vector<float> positions;
	void BeforDraw();
	void Draw();

	void Events();

public:
	Client();
	~Client();

	void Init(uint16_t port);
	void ConnectTo(const IPaddress& ip_address);

	void DoStep();
};

#endif // CLIENT_H
