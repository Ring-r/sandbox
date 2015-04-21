from world import World
from entity import Entity

class Server:
	def __init__(self):
		self.frame_duration = 0.5 # seconds
		self.clients = dict()
		self.world = World((0, 0))
		self.entities = []

#	def restart_game(entities):
#		self.grid.clear()
#		self.grid.fill_random(fill_percent)
#		for entity in self.entities:
#			(entity.i, entity.j) = self.grid.find_random_empty()
#			entity.save_state() # TODO: Comment to see interesting effect.
#			world.connect(entity)

# -
	def update(self, frame_elapsed_time):
		if frame_elapsed_time >= self.frame_duration:
			self.world.update()
			self.send_entities_all()
			frame_elapsed_time -= frame_duration
			frame_start_time += frame_duration

	def check_permission(self, client):
		return client in self.clients
	
	def check_permission_admin(self, client):
		return True

	def receive_connect(self, client):
		if not client in self.clients:
			client_entity = Entity()
#			(client_entity.i, client_entity.j) = self.world.find_random_empty()
			client_entity.i = -2
			client_entity.j = -2
			client_entity.save_state()
			self.clients[client] = client_entity
			self.entities.append(client_entity)

			self.send_world(client)
			self.send_entities_all()

	def receive_create_random_world(self, client, i_count, j_count, fill_percent):
		if not self.check_permission_admin(client):
			return

		self.world = World((i_count, j_count))
		self.world.fill_random(fill_percent)
		for client in self.clients:
			client_entity = self.clients[client]
			(client_entity.i, client_entity.j) = self.world.find_random_empty() # WARNING: Doesn't use inormation about other entities.

		self.send_world_all()
		self.send_entities_all()

	def receive_add_bot(self, client, bot_count):
		if not self.check_permission_admin(client):
			return

		for i in range(bot_count):
			bot_entity = Entity()
#			(bot_entity.i, bot_entity.j) = self.world.find_random_empty()
			bot_entity.i = -2
			bot_entity.j = -2
			bot_entity.save_state()
			self.entities.append(bot_entity)

		self.send_entities_all()

	def receive_restart_game(self, client):
		if not self.check_permission_admin(client):
			return

		self.receive_create_random_world(self, self.world.i_count, self.world.j_count, self.world.fill_percent)

	def send_world(self, client):
		client.receive_world(self, self.world)

	def send_world_all(self):
		for client in self.clients:
			self.send_world(client)

	def send_entities(self, client):
		client.receive_entities(self, self.entities)

	def send_entities_all(self):
		for client in self.clients:
			self.send_entities(client)

