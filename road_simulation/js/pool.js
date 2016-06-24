var Pool = function() {
	this.length = 0;
	this.capacity = 0;
}

Pool.prototype = new Array();
Pool.constructor = Pool;

Pool.prototype.init = function(capacity, template) {
	this.capacity = capacity;
	for (var i = 0; i < this.capacity; i++) {
		this[i] = new template();
	}
}

Pool.prototype.add = function() {
	if (this.length >= this.capacity) {
		return null;
	}
	
	var item = this[this.length];
	this.length++;
	return item;
}

Pool.prototype.del = function(index) {
	if (index < 0 || this.length <= index) {
		return null;
	}
	
	this.length--;
	var item = this[index];
	this[index] = this[this.length];
	this[this.length] = item;
	return item;
}
