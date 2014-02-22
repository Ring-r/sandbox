CC = g++ -Wall -ansi -std=c++11

all: base game
	$(CC) main.cpp debug/*.o -o debug/base -lSDL2 -lSDL_net -lSDL_ttf

base:
	$(CC) -c base.cpp -o debug/base.o

game: viewer_sdl level
	$(CC) -c game.cpp -o debug/game.o

viewer_sdl:
	$(CC) -c viewer_sdl.cpp -o debug/viewer_sdl.o

level: hero
	$(CC) -c level.cpp -o debug/level.o

hero: entity
	$(CC) -c hero.cpp -o debug/hero.o

entity:
	$(CC) -c entity.cpp -o debug/entity.o

clean:
	@echo Cleaning up...
	@rm debug/*.o
	@rm debug/base
	@echo Done.
