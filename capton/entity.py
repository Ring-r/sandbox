class Entity:
	def __init__(self):
		self.i = 0
		self.j = 0
		self.i_ = 1
		self.j_ = 0

		self.i_old = self.i
		self.j_old = self.j
		self.i__old = self.i_
		self.j__old = self.j_

	def move(self):
		self.i += self.i_
		self.j += self.j_

	def rotate(self):
		t = self.i_
		self.i_ = -self.j_
		self.j_ = t

	def update(self, world):
		self.i_old = self.i
		self.j_old = self.j
		self.i__old = self.i_
		self.j__old = self.j_

		i_new = self.i + self.i_
		j_new = self.j + self.j_

		if 0 <= i_new and i_new < world.i_count and 0 <= j_new and j_new < world.j_count and world.cells[i_new][j_new] == 0:
			self.move()
		else:
			self.rotate()

