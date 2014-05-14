CC = g++ -Wall -ansi -std=c++11

all: base game
	$(CC) main.cpp debug/*.o -o debug/base -lSDL2 -lSDL2_net -lSDL2_ttf

base:
	$(CC) -c base.cpp -o debug/base.o

game: viewer_sdl map map_factory map_viewer
	$(CC) -c game.cpp -o debug/game.o

viewer_sdl:
	$(CC) -c viewer_sdl.cpp -o debug/viewer_sdl.o

map:
	$(CC) -c map.cpp -o debug/map.o

map_factory: map
	$(CC) -c map_factory.cpp -o debug/map_factory.o

map_viewer: map
	$(CC) -c map_viewer.cpp -o debug/map_viewer.o

clean:
	@echo Cleaning up...
	@rm debug/*.o
	@rm debug/base
	@echo Done.
