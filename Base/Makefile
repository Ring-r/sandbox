CC = g++ -Wall -ansi -std=c++11

all: base game
	$(CC) main.cpp debug/*.o -o debug/base -lSDL2 -lSDL_net

base:
	$(CC) -c base.cpp -o debug/base.o

game: client server
	$(CC) -c game.cpp -o debug/game.o

listener:
	$(CC) -c listener.cpp -o debug/listener.o

client: sdl_viewer listener
	$(CC) -c client.cpp -o debug/client.o

sdl_viewer:
	$(CC) -c sdl_viewer.cpp -o debug/sdl_viewer.o

server: map listener
	$(CC) -c server.cpp -o debug/server.o

map:
	$(CC) -c map.cpp -o debug/map.o

clean:
	@echo Cleaning up...
	@rm lesson10
	@echo Done.
