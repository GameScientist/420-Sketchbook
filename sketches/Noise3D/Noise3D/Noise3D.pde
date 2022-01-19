ArrayList<PVector> blox = new ArrayList<PVector>();
float threshold = .5;
float zoom = 10;
float sizeOfBlocks = 10;
int dimOfBlocks = 5;
int numOfBlocks = 100;

void setup() {
  size(800, 500, P3D);
  noStroke();
  generateTerrainData();
}

void generateTerrainData() {
  blox.clear();
  float[][][]data = new float[dimOfBlocks][dimOfBlocks][dimOfBlocks];

  for (int x = 0; x < dimOfBlocks; x++)for (int y = 0; y < dimOfBlocks; y++)for (int z = 0; z < dimOfBlocks; z++)data[x][y][z]=noise(x/zoom, y/zoom, z/zoom) + y / 10;
  for (int x = 0; x < dimOfBlocks; x++)for (int y = 0; y < dimOfBlocks; y++)for (int z = 0; z < dimOfBlocks; z++)if (data[x][y][z]>0.5)blox.add(new PVector(x, y, z));
}

void checkInput() {
  boolean shouldRegen = false;

  if (Keys.PLUS()) {
    threshold += .01;
    shouldRegen = true;
  }

  if (Keys.MINUS()) {
    threshold -= .01;
    shouldRegen = true;
  }

  if (Keys.BRACKET_LEFT()) {
    zoom += .01;
    shouldRegen = true;
  }

  if (Keys.BRACKET_RIGHT()) {
    zoom -= .01;
    shouldRegen = true;
  }

  if (shouldRegen) { 
    threshold = constrain(threshold, 0, 1);
    zoom = constrain(zoom, 1, 50);
    generateTerrainData();
  }
}

void draw() {

  checkInput();
  background(0);
  lights();

  pushMatrix();
  translate(width/2, height/2);
  rotateX(map(mouseX, 0, width, -1, 1));
  rotateY(map(mousex\, 0, height, -PI, PI));
  translate(-dimOfBlocks*sizeOfBlocks/2, -dimOfBlocks*sizeOfBlocks/2);



  for (PVector pos : blox) { 
    pushMatrix();
    translate(pos.x * sizeOfBlocks, pos.y * sizeOfBlocks, pos.z * sizeOfBlocks);
    box(sizeOfBlocks, sizeOfBlocks, sizeOfBlocks);
    popMatrix();
  }

  popMatrix();
}
