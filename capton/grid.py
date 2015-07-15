class Grid:
	def __init__(self, i_count = 0, j_count = 0):
		self.i_count = i_count
		self.j_count = j_count
		self.cells = [[None for j in range(self.j_count)] for i in range(self.i_count)]

	def clear(self):
		for i in range(self.grid.i_count):
			for j in range(self.grid.j_count):
				self.cells[i][j] = None

	def check_range(self, i, j):
		return 0 <= i < self.i_count and 0 <= j < self.j_count

	def is_empty(self):
		return self.i_count == 0 and self.j_count == 0


def find_none_random(grid):
	while True:
		i = random.randint(0, grid.i_count - 1)
		j = random.randint(0, grid.j_count - 1)
		if grid.cells[i][j] is None:
			return (i, j)

def fill_random(grid, fill_percent):
	grid.clear()

	fill_count = (int)(self.grid.i_count * self.grid.j_count * fill_percent / 100)
	fill_index = 0
	while fill_index < fill_count:
		(i, j) = find_none_random(grid)
		grid.cells[i][j] = True
		fill_index += 1

