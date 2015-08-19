var canvas = document.createElement("canvas");
canvas.width = window.innerWidth;
canvas.height = window.innerHeight;
document.body.appendChild(canvas);
var context = canvas.getContext("2d");

var blocks = [];
var blockCount = 10;
for	(i = 0; i < blockCount; ++i) {
    blocks[i] = new Entity();
    var block = blocks[i];
    block.radius = 0.01;
    block.orbit = 0.5 * Math.random();
    block.angleSpeed = 0.01 * Math.random() - 0.005;
    block.fillStyle = 'red';
}

window.onload = main_loop();
function main_loop() {
  update();
  render();

  window.setTimeout(main_loop, 1000 / 60);
};

function update() {
  for	(i = 0; i < blockCount; ++i) {
    blocks[i].Update(1);
  }
}

function render() {
    context.fillStyle = '#ffffff'; // Options.backgroundColor
    context.fillRect(0, 0, canvas.width, canvas.height);

    context.save();
    context.translate(canvas.width / 2, canvas.height / 2);

    var scale = Math.min(canvas.width, canvas.height);

    for	(i = 0; i < blocks.length; ++i) {
      blocks[i].DrawOrbit(scale);
    }
    for	(i = 0; i < blocks.length; ++i) {
      blocks[i].Draw(scale);
    }

    context.restore();
};
