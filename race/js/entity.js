var Entity = function() {
	this.fillStyle = 'silver';
	this.strokeStyle = 'black';

	this.x = 0.0;
	this.x_size = 50.0;
	this.y = 0.0;
	this.y_size = 100.0;
	this.speed = 5.0;

	this.init = function(map_x_size, map_y_size) {
		this.x = (map_x_size - this.x_size) * Math.random();
		this.y = -this.y_size;
		return this;
	}

	this.update = function(timeEllapsed) {
		this.y += this.speed * timeEllapsed;
	}

	this.draw = function(scale) {
		context.beginPath();
		context.rect(this.x, this.y, this.x_size, this.y_size);

		context.fillStyle = this.fillStyle;
		context.fill();

		context.strokeStyle = this.strokeStyle;
		context.stroke();
	}
}
