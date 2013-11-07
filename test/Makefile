CC = g++ -Wall -ansi
LIB = -lGL -lGLU `sdl-config --cflags --libs`

all:
	$(CC) test.cpp -o test $(LIB)

clean:
	@echo Cleaning up...
	@rm test
	@echo Done.
