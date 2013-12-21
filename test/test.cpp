#include <iostream>
#include <climits>

#include <SDL/SDL.h>

#include <GL/gl.h>
#include <GL/glu.h>

#include "data.h"
//#include "utils.h"
#include "trackball.h"

const int SCREEN_BPP = 16;
const int SCREEN_WIDTH = 640;
const int SCREEN_HEIGHT = 480;

int videoFlags;
SDL_Surface* surface;
int Width, Height;

bool keyPress[INT_MAX];

GLfloat angle_x = 0.0f;
GLfloat angle_y = 0.0f;
GLfloat scale = 1.0f;

void Quit(int returnCode) {
  ClearData();
  SDL_Quit();
  exit(returnCode);
}

void Init() {
  if (SDL_Init(SDL_INIT_VIDEO) < 0) {
    std::cerr << "Video initialization failed: " << SDL_GetError();
    Quit(1);
  }

  SDL_WM_SetCaption("Test", 0);

  const SDL_VideoInfo* videoInfo = SDL_GetVideoInfo();
  if (!videoInfo) {
    std::cerr << "Video query failed: " << SDL_GetError();
    Quit(1);
  }

  videoFlags  = SDL_OPENGL | SDL_GL_DOUBLEBUFFER | SDL_HWPALETTE | SDL_RESIZABLE;
  if (videoInfo->hw_available) {
    videoFlags |= SDL_HWSURFACE;
  }
  else {
    videoFlags |= SDL_SWSURFACE;
  }
  if (videoInfo->blit_hw) {
    videoFlags |= SDL_HWACCEL;
  }

  SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);
}

void InitGl() {
  glShadeModel(GL_SMOOTH);
  glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
  glClearDepth(1.0f);
  glEnable(GL_DEPTH_TEST);
  glDepthFunc(GL_LEQUAL);
  glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);
  //glEnable(GL_CULL_FACE);
  //glCullFace(GL_FRONT_AND_BACK);
}

void CreateSurface(int width, int height) {
  surface = SDL_SetVideoMode(width, height, SCREEN_BPP, videoFlags);
  if (!surface) {
    std::cerr << "Video mode set failed: " << SDL_GetError();
    Quit(1);
  }
}

void ResizeWindow(int width, int height) {
  Width = width;
  Height = height;

  if (Height == 0) {
    Height = 1;    
  }

  glViewport(0, 0, (GLsizei)width, (GLsizei)height);

  glMatrixMode(GL_PROJECTION);
  glLoadIdentity();
  GLfloat ratio = (GLfloat)width / (GLfloat)height;
  GLfloat rr = r * ratio;
  glOrtho(x - rr, x + rr, y - r, y + r, z - r, z + r);
  //gluPerspective(45.0f, ratio, 0.1f, 100.0f);

  glMatrixMode(GL_MODELVIEW);
  glLoadIdentity();
}

void HandleKeyEvent(const SDL_KeyboardEvent* event) {
  keyPress[event->keysym.sym] = event->type == SDL_KEYDOWN;
  switch (event->keysym.sym) {
    case SDLK_ESCAPE:
      Quit(0);
      break;
    case SDLK_F1:
      SDL_WM_ToggleFullScreen(surface);
      break;
    default:
      break;
  }
}

void HandleVideoResize(const SDL_ResizeEvent* event) {
  CreateSurface(event->w, event->h);
  ResizeWindow(event->w, event->h);  
}

void Update() {
  if (keyPress[SDLK_DOWN]) {
    --angle_x;
  }
  if (keyPress[SDLK_LEFT]) {
    --angle_y;
  }
  if (keyPress[SDLK_RIGHT]) {
    ++angle_y;
  }
  if (keyPress[SDLK_UP]) {
    ++angle_x;
  }
  if (keyPress[SDLK_PAGEDOWN]) {
    scale *= 0.95f;
  }
  if (keyPress[SDLK_PAGEUP]) {
    scale *= 1.05f;
  }
}

void DrawAxes() {
  glBegin(GL_LINES);
  glColor3f(1.0f, 0.0f, 0.0f);
  glVertex3f(x, y, z);
  glVertex3f(x + r, y, z);

  glColor3f(0.0f, 1.0f, 0.0f);
  glVertex3f(x, y, z);
  glVertex3f(x, y + r, z);

  glColor3f(0.0f, 0.0f, 1.0f);
  glVertex3f(x, y, z);
  glVertex3f(x, y, z + r);
  glEnd();

}

void DrawGlScene() {
  ResizeWindow(Width, Height);
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

  glLoadIdentity();
  glTranslatef(-x, -y, -z);
  glScalef(scale, scale, scale);
  glRotatef(angle_x, 1.0f, 0.0f, 0.0f);
  glRotatef(angle_y, 0.0f, 1.0f, 0.0f);
  glTranslatef(x, y, z);
  
  DrawAxes();

  DrawData();

  SDL_GL_SwapBuffers();

  PrintFps();
}

int main(int argc, char **argv) {
  CreateData();

  Init();
  InitGl();
  CreateSurface(SCREEN_WIDTH, SCREEN_HEIGHT);
  ResizeWindow(SCREEN_WIDTH, SCREEN_HEIGHT);

  SDL_Event event;
  bool isActive = true;
  bool isFinish = false;
  while (!isFinish) {
    while (SDL_PollEvent(&event)) {
      switch(event.type) {
        case SDL_ACTIVEEVENT:
          isActive = event.active.gain != 0;
          break;			    
        case SDL_KEYDOWN:
          HandleKeyEvent(&event.key);
          break;
        case SDL_KEYUP:
          HandleKeyEvent(&event.key);
          break;
        case SDL_VIDEORESIZE:
          HandleVideoResize(&event.resize);
          break;
        case SDL_QUIT:
          isFinish = true;
          break;
        default:
          break;
      }
    }

    if (isActive) {
      Update();
      DrawGlScene();      
    }
  }

  Quit(0);
  return(0);
}
