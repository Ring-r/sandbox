#include <map>
#include <string>
#include <iostream>
#include <algorithm>
#include <windows.h>
#include <process.h>

unsigned int __stdcall Run(void* data);

class Listener {
private:
	bool quit;
	std::map<std::string, void(__thiscall Listener::*)()> commands;
	HANDLE thread;

	void QuitCmd() {
		std::cout << ">> Run QuitCmd()." << std::endl;
		this->quit = true;
	}
	void HelloCmd() {
		std::cout << ">> Run HelloCmd()." << std::endl;
	}

public:
	Listener()
		: quit(false) {
			this->commands["quit"] = &Listener::QuitCmd;
			this->commands["hello"] = &Listener::HelloCmd;

			unsigned threadId;
			this->thread = (HANDLE)_beginthreadex(nullptr, 1, Run, this, 0, &threadId);
	}

	~Listener() {
		this->quit = true;
	}

	bool GetQuit() {
		return this->quit;
	}

	friend unsigned int __stdcall Run(void* data);
};

unsigned int __stdcall Run(void* data) {
	Listener* listener = static_cast<Listener*>(data);
	while(!listener->quit) {
		std::cout << ": ";
		std::string str;
		std::getline(std::cin, str);
		std::transform(str.begin(), str.end(), str.begin(), ::tolower);
		if(listener->commands.find(str) != listener->commands.end()) {
			(listener->*(listener->commands[str]))();
		}
		else {
			std::cout << ">> Unknown command." << std::endl;
		}
		std::cout << std::endl;
	}
	return 0;
}

int main()
{
	std::cout << "Enter commands..." << std::endl;

	Listener listener;
	while(!listener.GetQuit()) {
		Sleep(1000);
	}

	return 0;
}