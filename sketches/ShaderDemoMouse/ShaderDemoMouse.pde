PImage img;
PShader shader;

void setup(){
  size(800, 600, P2D);
  
  img = loadImage("MugshotSpriteOutline.png");
  imageMode(CENTER);
  
  shader = loadShader("frag.glsl", "vert.glsl");
}

void draw(){
  //background(128);
  
  shader.set("mouse", mouseX/(float)width, 1 - mouseY/(float)height);
  filter(shader);
}
