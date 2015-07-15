import block
import grid
import random

class World:
	def __init__(self):
		self.grid = Grid()
		self.blocks = []
		self.entities = []

		self.blocks = [Block(0), Block(-1)]
		count = len(self.blocks)
		self.blocks_count_min = count
		self.blocks_count_max = count

	def _can_move_to(self, i, j):
		if not self.grid.check_range(entity.i, entity.j) or self.grid.cells[i][j] is None:
			return False
		return self.grid.cells[i][j].entities_count >= 0

	def fill_random(self, i_count, j_count, fill_percent):
		fill_random(self.grid, fill_percent)

		for entity in self.entities:
			(entity.i, entity.j) = find_empty_random(self.grid)
			self.grid.cells[entity.i][entity.j] = True
		for i in range(self.grid.i_count):
			for j in range(self.grid.j_count):
				if self.grid.cells[i][j] is None:
					self.grid.cells[i][j] = self.blocks[0]
				else
					self.grid.cells[i][j] = self.blocks[1]

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

# Check old block.
# Check entity.
# Check grid range.
# Check new block.

		for entity in self.entities:
			entity.can_move = True
			entity.save_state()
			entity.move() # CANDO: entity.move_unbordered(self.i_count, self.j_count)

		for entity in self.entities:
			if not self._can_move_to(entity.i, entity.j):
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

