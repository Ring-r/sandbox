CC = g++ -Wall -ansi -std=c++11

all:
	$(CC) base.cpp client.cpp client_main.cpp -o debug/client -lSDL2 -lSDL_net -Wl,-rpath=/usr/local/lib
	$(CC) server.cpp server_main.cpp -o debug/server -lSDL2 -lSDL_net -Wl,-rpath=/usr/local/lib

clean:
	@echo Cleaning up...
	@rm lesson10
	@echo Done.
