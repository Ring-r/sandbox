#!/usr/bin/python

import sys
import os
import pygame
import random
import time
import math

pygame.init()

back_color = (255, 255, 255)
controller_color = (192, 192, 192)
source_color = (0, 0, 255)
target_color = (255, 0, 0)
text_color = (10, 10, 10)

pygame.display.set_caption("Base")
resolution = (500, 300)
flag = 0 # pygame.FULLSCREEN
depth = 32
screen = pygame.display.set_mode(resolution, flag, depth)

dim = 2

acceleration = 0.5

radius = 32
source_point = [random.uniform(radius, x - radius) for x in resolution]
source_speed = 0.0
source_vector = [0.0, 0.0]
target_point = [random.uniform(2 * radius, x - 2 * radius) for x in resolution]

controller = [[0, 0], [0, 0]]
controller_enabled = False

show_text = False

while True:
	for event in pygame.event.get():
		if event.type == pygame.QUIT:
			pygame.quit()
			os._exit(0) # sys.exit(0)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				pygame.quit()
				os._exit(0) # sys.exit(0)
			if event.key == pygame.K_F1:
				show_text = not show_text
			if event.key == pygame.K_F5:
				source_point = [random.uniform(radius, x - radius) for x in resolution]
				source_speed = 0.0
				source_vector = [0.0, 0.0]
				target_point = [random.uniform(2 * radius, x - 2 * radius) for x in resolution]
		if event.type == pygame.MOUSEBUTTONDOWN:
			controller[0] = event.pos
			controller_enabled = True
		if event.type == pygame.MOUSEMOTION:
			controller[1] = event.pos
		if event.type == pygame.MOUSEBUTTONUP:
			controller[1] = event.pos
			controller_enabled = False
			source_speed = 0.0
			for i in range(dim):
				source_vector[i] = controller[1][i] - controller[0][i]
				source_speed += source_vector[i] * source_vector[i]
			source_speed = math.sqrt(source_speed)
			for i in range(dim):
				source_vector[i] /= source_speed

	for i in range(dim):
		source_point[i] += source_speed * source_vector[i]
	source_speed -= acceleration;
	if source_speed < 0:
		source_speed = 0.0

	for i in range(dim):
		if source_point[i] - radius < 0:
			source_vector[i] = abs(source_vector[i])
		if source_point[i] + radius > resolution[i]:
			source_vector[i] = -abs(source_vector[i])

	screen.fill(back_color)  
	pygame.draw.circle(screen, target_color, [int(round(x)) for x in target_point], radius)
	pygame.draw.circle(screen, target_color, [int(round(x)) for x in target_point], 2 * radius, 1)
	pygame.draw.circle(screen, source_color, [int(round(x)) for x in source_point], radius)
	if controller_enabled:
		pygame.draw.line(screen, controller_color, (controller[0][0], controller[0][1]), (controller[1][0], controller[1][1]))

	if show_text:
		font = pygame.font.Font(None, 16)
		text = font.render("Run...", 1, text_color)
		text_position = text.get_rect()
		screen.blit(text, text_position)

	pygame.display.update()

	time.sleep(1.0 / 60)

#if __name__ == "__main__":
#  main()
