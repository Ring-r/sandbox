#ifndef BOT_AUTO_1_HPP
#define BOT_AUTO_1_HPP

#include "_.hpp"

class BotAuto1 {
private:
	float px, vx;
	float py, vy;
	float angle;
	float speed;

  uint32_t min_x, max_x;
  uint32_t min_y, max_y;

public:
  BotAuto1(uint32_t min_x, uint32_t max_x, uint32_t min_y, uint32_t max_y) {
    this->min_x = min_x; this->max_x = max_x;
    this->min_y = min_y; this->max_y = max_y;

    this->px = rand() % (max_x - min_x) + min_x;
    this->py = rand() % (max_y - min_y) + min_y;

    this->angle = rand() % 360 * TO_RAD;
    this->speed = 3; // WARNING: Magic number.
    this->vx = this->speed * cos(this->angle);
    this->vy = this->speed * sin(this->angle);
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
    this->px += this->vx;
    if(this->px < min_x) {
      this->px = min_x + min_x - this->px;
      this->vx = fabs(this->vx);
    }
    if(this->px > max_x) {
      this->px = max_x + max_x - this->px;
      this->vx = -fabs(this->vx);
    }

    this->py += this->vy;
    if(this->py < min_y) {
      this->py = min_y + min_y - this->py;
      this->vy = fabs(this->vy);
    }
    if(this->py > max_y) {
      this->py = max_y + max_y - this->py;
      this->vy = -fabs(this->vy);
    }
  }
};

#endif // BOT_AUTO_1_HPP
