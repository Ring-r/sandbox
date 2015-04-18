import random

class World:
	def __init__(self, count = (0, 0)):
		self._max_block_count = 100 # 100 is a count of different types of world blocks. 
		self.i_count = count[0]
		self.j_count = count[1]
		self.cells = [[0 for i in range(self.i_count)] for j in range(self.j_count)]
	
	def fill_random(self, fill_percent):
		fill_count = self.i_count * self.j_count * fill_percent / 100
		for j in range(self.j_count):
			for i in range(self.i_count):
				self.cells[i][j] = 0
		for fill_index in range(fill_count):
			(i, j) = self.find_random_empty()
			self.cells[i][j] = 1
#		for entity_index in range(len(self.entities)):
#			(i, j) = self.find_random_empty()
#			self.cells[i][j] = entity_index + self._max_block_count
#			entity = self.entities[entity_index]
#			entity.i = i
#			entity.j = j
	
	def find_random_empty(self):
		while True:
			i = random.randint(0, self.i_count - 1)
			j = random.randint(0, self.j_count - 1)
			if self.cells[i][j] == 0:
				return (i, j)
	
	def update(self):
		for entity_index in range(len(self.entities)):
			entity = self.entities[entity_index]
			self.cells[entity.i][entity.j] = 0
			entity.update(self)
			self.cells[entity.i][entity.j] = entity_index + self._max_block_count

