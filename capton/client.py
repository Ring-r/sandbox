import pygame
import drawer
from world import World

class Client:
	def __init__(self):
		self.color_back = (0, 0, 0)

		self.world = World((0,0))
		self.entities = []

		self.cell_size = 20
		self.cell_border_size = 1
		self.cell_colors = ((12, 12, 12), (192, 192, 192))
		self.frame_duration = 0.5 # seconds
		self.entity_colors = [(255, 255, 255)]

		self.server = None

	def update(self, event):
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_ESCAPE:
				# TODO: Disconnect.
				pygame.quit()
				os._exit(0) # sys.exit(0)
			if event.key == pygame.K_F5:
				self.send_restart_game(self)
		if event.type == pygame.KEYUP:
			if event.key == pygame.K_RIGHT:
				self.send_entity_comand(self, 1)
			if event.key == pygame.K_DOWN:
				self.send_entity_comand(self, 2)
			if event.key == pygame.K_LEFT:
				self.send_entity_comand(self, 3)
			if event.key == pygame.K_UP:
				self.send_entity_comand(self, 4)
	
	def draw(self, screen, frame_elapsed_time):
		screen.fill(self.color_back)

		drawer.draw_grid(screen, self.world, self.cell_size, self.cell_border_size, self.cell_colors)

		coef = frame_elapsed_time / self.frame_duration if frame_elapsed_time < self.frame_duration else 1.0
		for entity_index in range(len(self.entities)):
			entity = self.entities[entity_index]
			color = self.entity_colors[entity_index]
			drawer.draw_entity(screen, entity, self.cell_size, self.cell_border_size, color, coef)

		pygame.display.update()

	def check_permission(self, server):
		return self.server == server

	def receive_world(self, server, world):
		if not self.check_permission(server):
			return
		
		self.world.i_count = world.i_count
		self.world.j_count = world.j_count
		if world.cells is None:
			self.world.cells = [[-1 for i in range(self.world.i_count)] for j in range(self.world.j_count)]
		else:
			self.world.cells = [[world.cells[i][j] for i in range(self.world.i_count)] for j in range(self.world.j_count)]

	def receive_entities(self, server, entities):
		if not self.check_permission(server):
			return

#		for i in range(bot_count):
#			self.entities.append(entity.Entity())
#			self.entity_colors.append((random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)))

	def send_connect(self, server):
		self.server = server
		self.server.receive_connect(self)

	def send_disconnect(self, server):
		self.server = None
		self.server.receive_disconnect(self)

	def send_create_random_world(self, i_count, j_count, fill_percent):
		self.server.receive_create_random_world(self, i_count, j_count, fill_percent)

	def send_add_bot(self, bot_count):
		self.server.receive_add_bot(self, bot_count)

	def send_restart_game(self):
		self.server.receive_restart_game(self)

