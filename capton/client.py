import os # import sys?
import pygame
import drawer
import random
import time
from world import World

class Client:
	def __init__(self):
		self.color_back = (0, 0, 0)

		self.world = World()
		self.entities = []

		self.cell_size = 20
		self.cell_border_size = 1
		self.cell_colors = ((12, 12, 12), (192, 192, 192))
		self.entity_colors = [(255, 255, 255)]

		self.server = None

		self.frame_duration = 0.5 # seconds
		self.frame_start_time = time.time() # TODO: Use in python3: time.perf_counter()
		self.frame_elapsed_time = 0

	def update(self):
		for event in pygame.event.get():
			if event.type == pygame.QUIT:
				pygame.quit()
				os._exit(0) # sys.exit(0)
			if event.type == pygame.KEYUP:
				if event.key == pygame.K_ESCAPE:
					# TODO: Disconnect.
					pygame.quit()
					os._exit(0) # sys.exit(0)
				if event.key == pygame.K_F5:
					self.send_restart_game()
			if event.type == pygame.KEYUP:
				if event.key == pygame.K_RIGHT:
					self.send_entity_command(1)
				if event.key == pygame.K_DOWN:
					self.send_entity_command(2)
				if event.key == pygame.K_LEFT:
					self.send_entity_command(3)
				if event.key == pygame.K_UP:
					self.send_entity_command(4)

	def draw(self, screen):
		screen.fill(self.color_back)

		drawer.draw_grid(screen, self.world, self.cell_size, self.cell_border_size, self.cell_colors)

		self.frame_elapsed_time = time.time() - self.frame_start_time # TODO: Use in python3: time.perf_counter()
		coef = self.frame_elapsed_time / self.frame_duration if self.frame_elapsed_time < self.frame_duration else 1.0
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
			self.world.cells = [[-1 for j in range(self.world.j_count)] for i in range(self.world.i_count)]
		else:
			self.world.cells = [[world.cells[i][j] for j in range(self.world.j_count)] for i in range(self.world.i_count)]

	def receive_entities(self, server, entities):
		if not self.check_permission(server):
			return

		self.frame_start_time = time.time()

		self.entities = []
		for entity in entities:
			self.entities.append(entity)
		diff_count = len(self.entities) - len(self.entity_colors)
		if diff_count > 0:
			for i in range(diff_count):
				self.entity_colors.append((random.randint(0, 255), random.randint(0, 255), random.randint(0, 255)))

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

	def send_entity_command(self, command):
		self.server.receive_entity_command(self, command)

