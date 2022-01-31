Dungeon d;

void setup(){
  size(800, 500);
}
void draw(){
  background(0);
  d.draw();
}

void mousePressed(){
  d.generate();
}
