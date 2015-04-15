import pygame

class Entity:
	def __init__(self):
		self.i = 0
		self.j = 0
		self.i_ = 1
		self.j_ = 0
	def move(self):
		self.i += self.i_
		self.j += self.j_
	def rotate(self):
		t = self.i_
		self.i_ = -self.j_
		self.j_ = t
	def update(self, world):
		i_new = self.i + self.i_
		j_new = self.j + self.j_
		if 0 <= i_new and i_new < world.i_count and 0 <= j_new and j_new < world.j_count and world.data[i_new][j_new] == 0:
			self.move()
		else:
			self.rotate()
	def draw(self, screen, cell_size, cell_border_size, color):
		x = self.i * cell_size + cell_border_size
		y = self.j * cell_size + cell_border_size
		pygame.draw.rect(screen, color, [x, y, cell_size - 2 * cell_border_size, cell_size - 2 * cell_border_size])
		
