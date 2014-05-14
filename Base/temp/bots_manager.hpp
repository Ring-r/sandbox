#ifndef BOTS_MANAGER_HPP
#define BOTS_MANAGER_HPP

#include "_.hpp"
#include "entity.hpp"

class BotsManager {
private:
  std::vector<Entity> entities;

  void RandomChange() {
    // TODO: Test code. Rewrite
    const int moveProcent = 20;
    if(rand() % 100 < moveProcent) {
      uint32_t i = rand() % this->entities.size();
      Entity& entity = this->entities[i];
      if (rand() % 100 < 50) {
	entity.angle = rand() % 360;
      }
      else {
	entity.speed = rand() % 4;
      }
      entity.vx = std::cos(entity.angle * TO_RAD) * entity.speed;
      entity.vy = std::sin(entity.angle * TO_RAD) * entity.speed;
    }
  }

public:
  BotsManager()
    : entities(0);

  void AddBots(uint8_t count, bool random_init = false) {
    this->entities.resize(this->entities.size() + count);
    // if(random_init) {
    //   InitsRandom(this->entities, 0.0f, 2 * this->screen_center_x, 0.0f, 2 * this->screen_center_y);
    // }
  }

  void DoStep() {
    this->RandomChange();
    for(auto it = this->entities.begin(); it < this->entities.end(); ++it) {
      it->DoStep();
    }
  }
};

#endif // BOTS_MANAGER_HPP
