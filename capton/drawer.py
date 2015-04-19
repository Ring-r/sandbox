import math
import pygame

def draw_grid(screen, grid, cell_size, cell_border_size, colors):
	for j in range(grid.j_count):
		for i in range(grid.i_count):
			cell_value = grid.cells[i][j]
			if cell_value >= 0:
				color = colors[cell_value]

				x = i * cell_size + cell_border_size
				y = j * cell_size + cell_border_size
				s = cell_size - 2 * cell_border_size

				pygame.draw.rect(screen, color, [x, y, s, s])

def draw_entity(screen, entity, cell_size, cell_border_size, color, coef):
	x =(entity.i_ + (entity.i - entity.i_) * coef) * cell_size + 0.5 * cell_size
	y =(entity.j_ + (entity.j - entity.j_) * coef) * cell_size + 0.5 * cell_size

	r = 0.5 * cell_size - cell_border_size

	points = [[x - r, y - r], [x + r, y - r], [x + r, y + r], [x - r, y + r]]
	pygame.draw.aalines(screen, color, True, points)

	angle_ = 2 * math.pi * entity.d_ / entity.DIRECTION_NUMBER
	angle = 2 * math.pi * entity.d / entity.DIRECTION_NUMBER
	if angle < angle_:
		angle += 2 * math.pi
	a = angle_ + (angle - angle_) * coef

	r -= cell_border_size

	rcosa = r * math.cos(a)
	rsina = r * math.sin(a)
	point_source = [x + rcosa, y + rsina]
	point_target = [x - rcosa, y - rsina]
	pygame.draw.aaline(screen, color, point_source, point_target)

	r /= 3.0
	shift = math.pi / 2

	point_target = [x + r * math.cos(a - shift), y + r * math.sin(a - shift)]
	pygame.draw.aaline(screen, color, point_source, point_target)
	point_target = [x + r * math.cos(a + shift), y + r * math.sin(a + shift)]
	pygame.draw.aaline(screen, color, point_source, point_target)

