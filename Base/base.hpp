#ifndef BASE_H
#define BASE_H

#include "_.hpp"

void LogError(const std::string& msg);
void LogSdlError(const std::string& msg);
void LogTtfError(const std::string& msg);

const float EPS = 0.0001f;

const float PI_F = static_cast<float>(M_PI);
const float TO_DEG = 180.0f / PI_F;
const float TO_RAD = PI_F / 180.0f;

#endif // BASE_H
