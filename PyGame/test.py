#!/usr/bin/python

import pyglet
from pyglet.gl import *

win = pyglet.window.Window()

@win.event
def on_draw():

	# Clear buffers
	glClear(GL_COLOR_BUFFER_BIT)

	# Draw outlines only
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE)

	# Draw some stuff
	glBegin(GL_TRIANGLES)
	glVertex3i(0, 0, 0)
	glVertex3i(300, 0, 0)
	glVertex3i(0, 300, 0)
	glEnd()

pyglet.app.run()
