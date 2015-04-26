import time
from world import World
from entity import Entity

class Server:
	def __init__(self):
		self.clients = dict()
		self.world = World()

		self.frame_duration = 0.5 # seconds
		self.frame_start_time = time.time() # TODO: Use in python3: time.perf_counter()
		self.frame_elapsed_time = 0

	def update(self):
		self.frame_elapsed_time = time.time() - self.frame_start_time # TODO: Use in python3: time.perf_counter()
		if self.frame_elapsed_time >= self.frame_duration:
			self.world.update()
			self.send_world_all()
			self.send_entities_all()
			self.frame_elapsed_time -= self.frame_duration
			self.frame_start_time += self.frame_duration

	def check_permission(self, client):
		return client in self.clients
	
	def check_permission_admin(self, client):
		return True

	def receive_connect(self, client):
		if not client in self.clients:
			entity = Entity((0, 0) if self.world.is_empty else self.world.find_random_empty)
			index = self.world.connect(entity)

			self.clients[client] = index

			self.send_world(client)
			self.send_entities_all()

	def receive_create_random_world(self, client, i_count, j_count, fill_percent):
		if not self.check_permission_admin(client):
			return

		self.world.fill_random(i_count, j_count, fill_percent)

		self.send_world_all()
		self.send_entities_all()

	def receive_add_bot(self, client, bot_count):
		if not self.check_permission_admin(client):
			return

		for i in range(bot_count):
			entity = Entity((0, 0) if self.world.is_empty else self.world.find_random_empty)
			self.world.connect(entity)

		self.send_entities_all()

	def receive_restart_game(self, client):
		if not self.check_permission_admin(client):
			return

		self.receive_create_random_world(self, self.world.i_count, self.world.j_count, self.world.fill_percent)

		self.frame_start_time = time.time() # TODO: Use in python3: time.perf_counter()
		self.frame_elapsed_time = 0

	def receive_entity_command(self, client, command):
		if not self.check_permission(client):
			return

		index = self.clients[client]
		self.world.entity_commands[index] = command

	def send_world(self, client):
		client.receive_world(self, self.world)

	def send_world_all(self):
		for client in self.clients:
			self.send_world(client)

	def send_entities(self, client):
		client.receive_entities(self, self.world.entities)

	def send_entities_all(self):
		for client in self.clients:
			self.send_entities(client)

