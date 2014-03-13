CC = g++ -Wall -ansi -std=c++11

all: base game
	$(CC) main.cpp debug/*.o -o debug/base -lSDL2 -lSDL_net -lSDL_ttf

base:
	$(CC) -c base.cpp -o debug/base.o

game: viewer_sdl level
	$(CC) -c game.cpp -o debug/game.o

viewer_sdl:
	$(CC) -c viewer_sdl.cpp -o debug/viewer_sdl.o

level: map enity entity_viewer entity_controller collision_controller
	$(CC) -c level.cpp -o debug/level.o

map:
	$(CC) -c map.cpp -o debug/map.o

entity_viewer: entity
	$(CC) -c entity_viewer.cpp -o debug/entity_viewer.o

entity_controller: entity
	$(CC) -c entity_controller.cpp -o debug/entity_controller.o

collision_controller: entity
	$(CC) -c collision_controller.cpp -o debug/collision_controller.o

entity:
	$(CC) -c entity.cpp -o debug/entity.o

clean:
	@echo Cleaning up...
	@rm debug/*.o
	@rm debug/base
	@echo Done.
