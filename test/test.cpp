#include <iostream>

#include <GL/gl.h>
#include <GL/glu.h>
#include <SDL/SDL.h>

#include "trackball.h"

#define SCREEN_WIDTH  640
#define SCREEN_HEIGHT 480
#define SCREEN_BPP     16

SDL_Surface* surface;

void Quit(int returnCode) {
  SDL_Quit();
  exit(returnCode);
}

void ResizeWindow(int width, int height) {
  if (height == 0)
    height = 1;

  GLfloat ratio = (GLfloat)width / (GLfloat)height;

  glViewport(0, 0, (GLsizei)width, (GLsizei)height);
  glMatrixMode(GL_PROJECTION);
  glLoadIdentity();

  gluPerspective(45.0f, ratio, 0.1f, 100.0f);
  glMatrixMode(GL_MODELVIEW);
  glLoadIdentity();
}

void HandleKeyPress(SDL_keysym *keysym) {
  switch (keysym->sym) {
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

void InitGL() {
  glShadeModel(GL_SMOOTH);

  glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

  glClearDepth(1.0f);

  glEnable(GL_DEPTH_TEST);

  glDepthFunc(GL_LEQUAL);

  glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);
}

void DrawGLScene() {
  static GLfloat rtri, rquad;
  static GLint T0     = 0;
  static GLint Frames = 0;


  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

  glLoadIdentity();
  glTranslatef(0.0f, 0.0f, -6.0f);
  glRotatef(rtri, 0.0f, 1.0f, 0.0f);

  float m[4][4];
  Fill(0.0f, 0.0f, 0.0f, 1.0f, m);
  glMultMatrixf(&(m[0][0]));

  glBegin(GL_TRIANGLES);             /* Drawing Using Triangles       */
    glColor3f(  1.0f,  0.0f,  0.0f); /* Red                           */
    glVertex3f( 0.0f,  1.0f,  0.0f); /* Top Of Triangle (Front)       */
    glColor3f(  0.0f,  1.0f,  0.0f); /* Green                         */
    glVertex3f(-1.0f, -1.0f,  1.0f); /* Left Of Triangle (Front)      */
    glColor3f(  0.0f,  0.0f,  1.0f); /* Blue                          */
    glVertex3f( 1.0f, -1.0f,  1.0f); /* Right Of Triangle (Front)     */

    glColor3f(  1.0f,  0.0f,  0.0f); /* Red                           */
    glVertex3f( 0.0f,  1.0f,  0.0f); /* Top Of Triangle (Right)       */
    glColor3f(  0.0f,  0.0f,  1.0f); /* Blue                          */
    glVertex3f( 1.0f, -1.0f,  1.0f); /* Left Of Triangle (Right)      */
    glColor3f(  0.0f,  1.0f,  0.0f); /* Green                         */
    glVertex3f( 1.0f, -1.0f, -1.0f); /* Right Of Triangle (Right)     */

    glColor3f(  1.0f,  0.0f,  0.0f); /* Red                           */
    glVertex3f( 0.0f,  1.0f,  0.0f); /* Top Of Triangle (Back)        */
    glColor3f(  0.0f,  1.0f,  0.0f); /* Green                         */
    glVertex3f( 1.0f, -1.0f, -1.0f); /* Left Of Triangle (Back)       */
    glColor3f(  0.0f,  0.0f,  1.0f); /* Blue                          */
    glVertex3f(-1.0f, -1.0f, -1.0f); /* Right Of Triangle (Back)      */

    glColor3f(  1.0f,  0.0f,  0.0f); /* Red                           */
    glVertex3f( 0.0f,  1.0f,  0.0f); /* Top Of Triangle (Left)        */
    glColor3f(  0.0f,  0.0f,  1.0f); /* Blue                          */
    glVertex3f(-1.0f, -1.0f, -1.0f); /* Left Of Triangle (Left)       */
    glColor3f(  0.0f,  1.0f,  0.0f); /* Green                         */
    glVertex3f(-1.0f, -1.0f,  1.0f); /* Right Of Triangle (Left)      */
  glEnd();                           /* Finished Drawing The Triangle */

  SDL_GL_SwapBuffers();

  Frames++;
  {
    GLint t = SDL_GetTicks();
    if (t - T0 >= 5000) {
      GLfloat seconds = (t - T0) / 1000.0;
      GLfloat fps = Frames / seconds;
      printf("%d frames in %g seconds = %g FPS\n", Frames, seconds, fps);
      T0 = t;
      Frames = 0;
    }
  }

  rtri  += 0.2f;
  rquad -=0.15f;
}

int main(int argc, char **argv) {
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

  int videoFlags  = SDL_OPENGL | SDL_GL_DOUBLEBUFFER | SDL_HWPALETTE | SDL_RESIZABLE;
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

  surface = SDL_SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, SCREEN_BPP, videoFlags);
  if (!surface) {
    std::cerr << "Video mode set failed: " << SDL_GetError();
    Quit(1);
  }

  InitGL();

  ResizeWindow(SCREEN_WIDTH, SCREEN_HEIGHT);

  SDL_Event event;
  bool isActive = true;
  bool done = false;
  while (!done) {
    while (SDL_PollEvent(&event)) {
      switch(event.type) {
        case SDL_ACTIVEEVENT:
          isActive = event.active.gain != 0;
          break;			    
        case SDL_VIDEORESIZE:
          surface = SDL_SetVideoMode(event.resize.w, event.resize.h, SCREEN_BPP, videoFlags);
          if (!surface) {
            std::cerr << "Could not get a surface after resize: " << SDL_GetError();
            Quit(1);
          }
          ResizeWindow(event.resize.w, event.resize.h);
          break;
        case SDL_KEYDOWN:
          HandleKeyPress(&event.key.keysym);
          break;
        case SDL_QUIT:
          done = true;
          break;
        default:
          break;
      }
    }

    if (isActive)
      DrawGLScene();
  }

  Quit(0);
  return(0);
}
