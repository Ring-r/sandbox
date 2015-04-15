import entity
import pygame
import random

class World:
	def __init__(self):
		self._max_block_count = 100 # 100 is a count of different types of world blocks. 
		self.i_count = 0
		self.j_count = 0
	def init(self, i_count, j_count, entity_count):
		self.i_count = i_count
		self.j_count = j_count
		self.data = [[0 for i in range(self.i_count)] for j in range(self.j_count)]
		self.entities = [entity.Entity() for entity_index in range(entity_count)]
	def fill_random(self, fill_percent):
		fill_count = self.i_count * self.j_count * fill_percent / 100
		for fill_index in range(fill_count):
			(i, j) = self.find_random_empty()
			self.data[i][j] = 1
		for entity_index in range(len(self.entities)):
			(i, j) = self.find_random_empty()
			self.data[i][j] = entity_index + self._max_block_count
			entity = self.entities[entity_index]
			entity.i = i
			entity.j = j
	def find_random_empty(self):
		while True:
			i = random.randint(0, self.i_count - 1)
			j = random.randint(0, self.j_count - 1)
			if self.data[i][j] == 0:
				return (i, j)
	def update(self):
		for entity_index in range(len(self.entities)):
			entity = self.entities[entity_index]
			self.data[entity.i][entity.j] = 0
			entity.update(self)
			self.data[entity.i][entity.j] = entity_index + self._max_block_count
	def draw(self, screen, cell_size, cell_border_size, block_colors, entity_colors):
		for j in range(self.j_count):
			for i in range(self.i_count):
				k = self.data[i][j]
				if k < 100:
					color = block_colors[k]
				else:
					color = entity_colors[k - 100]
				x = i * cell_size + cell_border_size
				y = j * cell_size + cell_border_size
				pygame.draw.rect(screen, color, [x, y, cell_size - 2 * cell_border_size, cell_size - 2 * cell_border_size])

