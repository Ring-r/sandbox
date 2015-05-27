class Entity:
	DIRECTION_NUMBER = 4

	def __init__(self, position = (0, 0), direction_index = 0):
		self.i = position[0]
		self.j = position[1]
		self.d = direction_index

		self.save_state()

		self.command = 0

	def get_direction(self):
		if self.d == 0:
			return (1, 0)
		if self.d == 1:
			return (0, 1)
		if self.d == 2:
			return (-1, 0)
		if self.d == 3:
			return (0, -1)

	def move(self):
		(i, j) = self.get_direction()
		self.i += i
		self.j += j

	def rotate(self):
		self.d = (self.d + 1) % Entity.DIRECTION_NUMBER

	def save_state(self):
		self.i_ = self.i
		self.j_ = self.j
		self.d_ = self.d

	def restore_state(self):
		self.i = self.i_
		self.j = self.j_
		self.d = self.d_

	def is_moved(self):
		return self.i != self.i_ or self.j != self.j_

