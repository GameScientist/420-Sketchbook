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
  pushMatrix();
  translate(mouseX, mouseY);
  scale(.25);
  image(img, 0, 0);
  popMatrix();
  filter(shader);
}