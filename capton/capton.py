#!/usr/bin/python

import sys
import os
import pygame
import time

title = "capton"
resolution = (600, 600)
flag = 0 # pygame.FULLSCREEN
depth = 32
back_color = (0, 0, 0)


pygame.init()
pygame.display.set_caption(title)
screen = pygame.display.set_mode(resolution, flag, depth)

while True:
	for event in pygame.event.get():
		if event.type == pygame.QUIT:
			pygame.quit()
			os._exit(0) # sys.exit(0)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				pygame.quit()
				os._exit(0) # sys.exit(0)

	screen.fill(back_color)  

	pygame.display.update()

	time.sleep(1 / 60)

#if __name__ == "__main__":
#  main()
