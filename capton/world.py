import pygame
import random

class World:
	def __init__(self):
		self.i_count = 0
		self.j_count = 0
	def init(self, i_count, j_count):
		self.i_count = i_count
		self.j_count = j_count
		self.data = [[0 for i in range(self.i_count)] for j in range(self.j_count)]
	def fill_random(self, fill_percent):
		fill_count = self.i_count * self.j_count * fill_percent / 100
		for k in range(fill_count):
			i = random.randint(0, self.i_count - 1)
			j = random.randint(0, self.j_count - 1)
			self.data[i][j] = 1
	def find_empty(self):
		while True:
			i = random.randint(0, self.i_count)
			j = random.randint(0, self.j_count)
			if self.data[i][j] == 0:
				return (i, j)
	def draw(self, screen, color_back, color_front, cell_size, cell_border_size):
		for j in range(self.j_count):
			for i in range(self.i_count):
				x = i * cell_size + cell_border_size
				y = j * cell_size + cell_border_size
				color = color_back
				if self.data[i][j] > 0:
					color = color_front
				pygame.draw.rect(screen, color, [x, y, cell_size - 2 * cell_border_size, cell_size - 2 * cell_border_size])

