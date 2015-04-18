import pygame

def draw(screen, grid, cell_size, cell_border_size, colors):
	for j in range(grid.j_count):
		for i in range(grid.i_count):
			cell_value = grid.cells[i][j]
			if cell_value >= 0:
				color = colors[cell_value]

				x = i * cell_size + cell_border_size
				y = j * cell_size + cell_border_size
				s = cell_size - 2 * cell_border_size

				pygame.draw.rect(screen, color, [x, y, s, s])

