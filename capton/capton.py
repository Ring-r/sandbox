#!/usr/bin/python

from server import Server
from client import Client
import pygame
import time

def main():
	server = Server()

	title = "capton"
	resolution = (600, 600)
	flag = 0 # pygame.FULLSCREEN
	depth = 32

	pygame.init()
	pygame.display.set_caption(title)
	screen = pygame.display.set_mode(resolution, flag, depth)

	client = Client()

#	ENTITY_COLOR = (255, 255, 255)
#	client.set_entity_color(ENTITY_COLOR)

	client.send_connect(server)

	I_COUNT = 30
	J_COUNT = 30
	FILL_PERCENT = 20
	client.send_create_random_world(I_COUNT, J_COUNT, FILL_PERCENT)

	BOT_COUNT = 20
	client.send_add_bot(BOT_COUNT)

	client.send_restart_game()

	while True:
		server.update()

		client.update()

		client.draw(screen)

		time.sleep(1.0 / 60)

if __name__ == "__main__":
	main()

