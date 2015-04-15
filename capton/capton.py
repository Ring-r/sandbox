#!/usr/bin/python

import sys
import os
import pygame
import time
import world
import entity

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
colors = ((12, 12, 12), (192, 192, 192))
color_entity = (0, 0, 255)

world = world.World()
world.init(i_count, j_count)
world.fill_random(fill_percent)

entity = entity.Entity()
(entity.i, entity.j) = world.find_empty()

while True:
	for event in pygame.event.get():
		if event.type == pygame.QUIT:
			pygame.quit()
			os._exit(0) # sys.exit(0)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				pygame.quit()
				os._exit(0) # sys.exit(0)

	entity.update(world)

	screen.fill(color_back)

	world.draw(screen, cell_size, cell_border_size, colors)

	entity.draw(screen, cell_size, cell_border_size, color_entity)

	pygame.display.update()

	time.sleep(1.0 / 60)

#if __name__ == "__main__":
#  main()
