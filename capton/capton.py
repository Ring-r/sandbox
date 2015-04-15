#!/usr/bin/python

import sys
import os
import pygame
import time
import world

title = "capton"
resolution = (600, 600)
flag = 0 # pygame.FULLSCREEN
depth = 32
color_back = (0, 0, 0)

pygame.init()
pygame.display.set_caption(title)
screen = pygame.display.set_mode(resolution, flag, depth)

i_count = 30
j_count = 30
fill_percent = 20
cell_size = 20
cell_border_size = 1
color_empty = (12, 12, 12)
color_block = (192, 192, 192)

world = world.World()
world.init(i_count, j_count)
world.fill_random(fill_percent)

while True:
	for event in pygame.event.get():
		if event.type == pygame.QUIT:
			pygame.quit()
			os._exit(0) # sys.exit(0)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				pygame.quit()
				os._exit(0) # sys.exit(0)

	screen.fill(color_back)

	world.draw(screen, color_empty, color_block, cell_size, cell_border_size)

	pygame.display.update()

	time.sleep(1 / 60)

#if __name__ == "__main__":
#  main()
