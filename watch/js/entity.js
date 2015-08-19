
function VisaulizeCircle(x, y, r, fillStyle, strokeStyle) {
  context.beginPath();
  context.arc(x, y, r, 0, 2 * Math.PI);

  context.fillStyle = fillStyle;
  context.fill();

  context.strokeStyle = strokeStyle;
  context.stroke();
}

var Entity  = function() {
  this.fillStyle = 'black';
	this.strokeStyle = 'black';
  this.orbitFillStyle = 'transparent';
	this.orbitStrokeStyle = 'silver';

	this.angle = 0.0;
	this.angleSpeed = 0.0;
	this.orbit = 0.0;
	this.orbitSpeed = 0.0;

	this.radius = 0.0;

	this.next = null;

	this.Update = function(timeEllapsed) {
		this.angle += this.angleSpeed * timeEllapsed;
		this.orbit += this.orbitSpeed * timeEllapsed;
		this.orbit = Math.max(this.orbit, 0.0);
	}

	this.Draw = function(scale) {
		var x = scale * this.GetX();
		var y = scale * this.GetY();
		var r = scale * this.radius;
    VisaulizeCircle(x, y, r, this.fillStyle, this.strokeStyle);
	}

	this.DrawOrbit = function(scale) {
    var x = scale * 0.0;
		var y = scale * 0.0;
		var r = scale * this.orbit;
    VisaulizeCircle(x, y, r, this.orbitFillStyle, this.orbitStrokeStyle);
	}

	this.GetX = function() {
		return this.orbit * Math.cos(this.angle);
	}

	this.GetY = function() {
		return this.orbit * Math.sin(this.angle);
	}

	this.GetDistance = function(entity) {
		var x = this.GetX() - entity.GetX();
		var y = this.GetY() - entity.GetY();
		return Math.sqrt(x * x + y * y);
	}
}
