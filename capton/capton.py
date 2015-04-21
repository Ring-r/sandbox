#!/usr/bin/python

import os # import sys?
from server import Server
from client import Client
import pygame
import drawer
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
#	client.entity_colors[0] = ENTITY_COLOR

	client.send_connect(server)

	I_COUNT = 30
	J_COUNT = 30
	FILL_PERCENT = 20
	client.send_create_random_world(I_COUNT, J_COUNT, FILL_PERCENT)

	BOT_COUNT = 20
	client.send_add_bot(BOT_COUNT)

	client.send_restart_game()

	frame_start_time = time.time() # TODO: Use in python3: time.perf_counter()
	while True:
		for event in pygame.event.get():
			if event.type == pygame.QUIT:
				pygame.quit()
				os._exit(0) # sys.exit(0)

			client.update(event)

		frame_elapsed_time = time.time() - frame_start_time # TODO: Use in python3: time.perf_counter()
		server.update(frame_elapsed_time)
		client.draw(screen, frame_elapsed_time)

		time.sleep(1.0 / 60)

if __name__ == "__main__":
	main()

