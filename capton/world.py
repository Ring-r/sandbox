import random

class Block:
	def __init__(self, entities_count = 0):
		self.entities_count = entities_count
		self.position = (-1, -1)
		pass

class World:
	def __init__(self, count = (0, 0)):
		self.i_count = count[0]
		self.j_count = count[1]
		self.cells = [[0 for j in range(self.j_count)] for i in range(self.i_count)]
		self.fill_percent = 0

		self.entities = []

		self.blocks = [Block(0), Block(-1)]
		count = len(self.blocks)
		self.blocks_count_min = count
		self.blocks_count_max = count

	def is_empty(self):
		return self.i_count == 0 and self.j_count == 0

	def check_range(self, i, j):
		return 0 <= i < self.i_count and 0 <= j < self.j_count

	def check_free(self, i, j):
		index = self.cells[i][j]
		block = self.blocks[index]
		return block.entities_count >= 0

	def find_random_empty(self):
		while True:
			i = random.randint(0, self.i_count - 1)
			j = random.randint(0, self.j_count - 1)
			if self.cells[i][j] == 0:
				return (i, j)

	def fill_random(self, i_count, j_count, fill_percent):
		self.i_count = i_count
		self.j_count = j_count
		self.cells = [[0 for j in range(self.j_count)] for i in range(self.i_count)]
		self.fill_percent = fill_percent

		fill_count = (int)(self.i_count * self.j_count * self.fill_percent / 100)
		for fill_index in range(fill_count):
			(i, j) = self.find_random_empty()
			self.cells[i][j] = 1
		for entity in self.entities:
			(entity.i, entity.j) = self.find_random_empty()
			self.cells[entity.i][entity.j] = 1
		for entity in self.entities:
			self.cells[entity.i][entity.j] = 0

	def connect(self, entity):
		self.entities.append(entity)

		return len(self.entities) - 1

	def raise_commands(self):
		for entity in self.entities:
			if entity.command > 0:
				entity.d = entity.command - 1
			entity.command = 0

	def create_temp_block_index(self):
		if self.blocks_count_max == len(self.blocks):
			self.blocks.append(Block())

		self.blocks_count_max += 1
		return self.blocks_count_max -1

	def update(self):
		self.raise_commands()

		for entity in self.entities:
			entity.can_move = True
			entity.save_state()
			entity.move() # CANDO: entity.move_unbordered(self.i_count, self.j_count)

		for entity in self.entities:
			if not self.check_range(entity.i, entity.j) or not self.check_free(entity.i, entity.j):
				entity.can_move = False
				entity.restore_state()

			block_index = self.cells[entity.i][entity.j]
			block = self.blocks[block_index]
			if block.entities_count == 0:
				block_index = self.create_temp_block_index()
				block = self.blocks[block_index]
				block.entities_count = 1
				block.position = (entity.i, entity.j)
				self.cells[entity.i][entity.j] = block_index 
			else:
				block.entities_count += 1

		for entity in self.entities:
			if entity.can_move:
				block_index = self.cells[entity.i][entity.j]
				block = self.blocks[block_index]
				entity.can_move = block.entities_count == 1

		index = self.blocks_count_min
		while (index < self.blocks_count_max):
			(i, j) = self.blocks[index].position
			self.cells[i][j] = 0
			index += 1
		self.blocks_count_max = self.blocks_count_min

		for entity in self.entities:
			if not entity.can_move:
				entity.restore_state()
				entity.rotate()

