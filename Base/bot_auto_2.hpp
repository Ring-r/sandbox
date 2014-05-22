#ifndef BOT_AUTO_2_HPP
#define BOT_AUTO_2_HPP

#include "_.hpp"

class BotAuto2 {
private:
	float px, py;
	float angle;

	float speed;
	float speed_angle;

	float nx, ny;

	uint32_t min_x, max_x;
	uint32_t min_y, max_y;

public:
	BotAuto2(uint32_t min_x, uint32_t max_x, uint32_t min_y, uint32_t max_y) {
		this->min_x = min_x; this->max_x = max_x;
		this->min_y = min_y; this->max_y = max_y;

		this->px = rand() % (max_x - min_x) + min_x;
		this->py = rand() % (max_y - min_y) + min_y;
		this->angle = rand() % 360 * TO_RAD;

		this->speed = 3.0f; // WARNING: Magic number.
		this->speed_angle = 0.03f; // WARNING: Magic number.

		this->nx = rand() % (max_x - min_x) + min_x;
		this->ny = rand() % (max_y - min_y) + min_y;
	}

	float GetAngle() const {
		return this->angle;
	}

	float GetPx() const {
		return this->px;
	}

	float GetPy() const {
		return this->py;
	}

	void DoStep() {
		float x = this->nx - this->px;
		float y = this->ny - this->py;
		float angle = atan2(x, y) + PI_F;
		while(this->angle > angle) {
			this->angle -= 2 * PI_F;
		}
		if(angle - this->angle > PI_F) {
			this->angle += 2 * PI_F;
		}

		if(fabs(this->angle - angle) > this->speed_angle) {
			this->angle += angle > this->angle ? this->speed_angle : -this->speed_angle;
		}
		else {
			this->angle = angle;
			if(fabs(x) > this->speed || fabs(y) > this->speed) {
				float d = sqrt(x * x + y * y);
				this->px += x * this->speed / d;
				this->py += y * this->speed / d;
			}
			else {
				this->px = this->nx;
				this->py = this->ny;
				this->nx = rand() % (max_x - min_x) + min_x;
				this->ny = rand() % (max_y - min_y) + min_y;
			}
		}
	}
};

#endif // BOT_AUTO_2_HPP
