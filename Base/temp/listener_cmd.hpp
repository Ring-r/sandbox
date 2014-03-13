#ifndef LISTENER_CMD_H
#define LISTENER_CMD_H

#include <map>
#include <vector>

class ListenerCmd {
protected:
	bool init;
	std::vector<char> buffer;
	std::map<std::string, void(ListenerCmd::*)()> commands;

	void Clear();

public:
	ListenerCmd();
	~ListenerCmd();

	void Init();
	void DoStep();
};

#endif // LISTENER_CMD_H
