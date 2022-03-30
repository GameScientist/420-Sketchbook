PImage img;
PShader shader;

void setup(){
  size(800, 600, P2D);
  
  img = loadImage("MugshotSpriteOutline.png");
  imageMode(CENTER);
  
  shader = loadShader("frag.glsl", "vert.glsl");
  
  shader(shader);
}

void draw(){
  background(128);
  image(img, mouseX, mouseY);
  //filter(shader);
}
