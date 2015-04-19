import random

class World:
	def __init__(self, count = (0, 0)):
		self.i_count = count[0]
		self.j_count = count[1]
		self.cells = [[0 for i in range(self.i_count)] for j in range(self.j_count)]

		self.clear()

	def check_range(self, i, j):
		return 0 <= i and i < self.i_count and 0 <= j and j < self.j_count

	def clear(self):
		for j in range(self.j_count):
			for i in range(self.i_count):
				self.cells[i][j] = 0

		self.entities = []
		self.entity_commands = []

	def find_random_empty(self):
		while True:
			i = random.randint(0, self.i_count - 1)
			j = random.randint(0, self.j_count - 1)
			if self.cells[i][j] == 0:
				return (i, j)

	def fill_random(self, fill_percent):
		fill_count = self.i_count * self.j_count * fill_percent / 100
		for fill_index in range(fill_count):
			(i, j) = self.find_random_empty()
			self.cells[i][j] = 1

	def connect(self, entity):
		self.entities.append(entity)
		self.entity_commands.append(0)
	
	def send_command_to_entity(self, entity_index, command_index):
		# TODO: Check permission.
		self.entity_commands[entity_index] = command_index

	def update(self):
		def raise_command(entity, command):
			if command > 0:
				entity.d = command - 1

		for entity in self.entities:
			entity.can_move = True

		temp_cells = [[-1 for i in range(self.i_count)] for j in range(self.j_count)]
		for entity_index in range(len(self.entities)):
			entity = self.entities[entity_index]
			command = self.entity_commands[entity_index]
			raise_command(entity, command)
			self.entity_commands[entity_index] = 0

			entity.save_state()
			entity.move() # TODO: Can use unbordered world there.

			if not self.check_range(entity.i, entity.j) or self.cells[entity.i][entity.j] > 0:
				entity.can_move = False
				entity.restore_state()

			temp_entity_index = temp_cells[entity.i][entity.j]
			if temp_entity_index < 0:
				temp_cells[entity.i][entity.j] = entity_index
			else:
				self.entities[temp_entity_index].can_move = False
				entity.can_move = False

		for entity in self.entities:
			if not entity.can_move:
				entity.restore_state()
				entity.rotate()

