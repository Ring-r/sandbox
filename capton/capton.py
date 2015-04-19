#!/usr/bin/python

import sys
import os
import pygame
import random
import time
import world
import entity
import drawer

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
cell_colors = ((12, 12, 12), (192, 192, 192))

world = world.World((i_count, j_count))

border_color = (250, 0, 0)
entity_count = 20
entity_colors = []
for i in range(entity_count):
	entity_colors.append((random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)))

entities = [entity.Entity() for i in range(entity_count)]

current_entity_index = 0
entity_colors[current_entity_index] = (255, 255, 255)

def init(world, entities):
	world.clear()
	world.fill_random(fill_percent)
	for entity in entities:
		(entity.i, entity.j) = world.find_random_empty()
		entity.save_state() # TODO: Comment to see interesting effect.
		world.connect(entity)

init(world, entities)

frame_duration = 0.5 # seconds
frame_start_time = time.time() # TODO: Use in python3: time.perf_counter()

while True:
	for event in pygame.event.get():
		if event.type == pygame.QUIT:
			pygame.quit()
			os._exit(0) # sys.exit(0)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				pygame.quit()
				os._exit(0) # sys.exit(0)
			if event.key == pygame.K_F5:
				init(world, entities)

		if event.type == pygame.KEYUP:
			if event.key == pygame.K_RIGHT:
				world.send_command_to_entity(current_entity_index, 1)
			if event.key == pygame.K_DOWN:
				world.send_command_to_entity(current_entity_index, 2)
			if event.key == pygame.K_LEFT:
				world.send_command_to_entity(current_entity_index, 3)
			if event.key == pygame.K_UP:
				world.send_command_to_entity(current_entity_index, 4)

	frame_elapsed_time = time.time() - frame_start_time # TODO: Use in python3: time.perf_counter()

	# Server part. ====>
	if frame_elapsed_time >= frame_duration:
		world.update()
		frame_elapsed_time -= frame_duration
		frame_start_time += frame_duration
	# ====< Server part.

	# Client part. ====>
	screen.fill(color_back)

	drawer.draw_grid(screen, world, cell_size, cell_border_size, cell_colors)

	coef = frame_elapsed_time / frame_duration if frame_elapsed_time < frame_duration else 1.0
	for entity_index in range(len(entities)):
		entity = entities[entity_index]
		color = entity_colors[entity_index]
		drawer.draw_entity(screen, entity, cell_size, cell_border_size, color, coef)

	pygame.display.update()
	# ====< Client part.

	time.sleep(1.0 / 60)

#if __name__ == "__main__":
#  main()

