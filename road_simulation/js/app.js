var canvas = document.createElement("canvas");
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;
document.body.appendChild(canvas);
var context = canvas.getContext("2d");

var entities = new Pool();
var entities_length_max = 10;
var next_time = 0;
var next_time_min = 5;
var next_time_max = 50;

window.onload = main();
function main() {
	init();
	main_loop();
}

function init() {
	this.entities.init(entities_length_max, Entity);
}

function main_loop() {
  update();
  render();

  window.setTimeout(main_loop, 1000 / 60);
};

function update() {
	if (next_time > 0) {
		next_time--;
	}
	else {
		var entity = entities.add();
		if (entity) {
			entity.init(canvas.width, canvas.height);
			next_time = next_time_min + Math.floor((next_time_max - next_time_min) * Math.random());
		}
	}

	for (var i = 0; i < entities.length; i++) {
		var entity = entities[i];
		entity.update(1);
		
		if (entity.y > canvas.height) {
			entities.del(i);
			i -= 1;
		}
	}
}

function render() {
	context.fillStyle = '#ffffff'; // Options.backgroundColor
	context.fillRect(0, 0, canvas.width, canvas.height);
	
	var scale = Math.min(canvas.width, canvas.height);
	
	for (var i = 0; i < entities.length; i++) {
		entities[i].draw(scale);
	}
};
