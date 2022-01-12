float[] valsWave;
float[] valsNoise;
float[] valsRand;

// Boxing:
ArrayList<PVector> vals = new ArrayList<PVector>();

void setup() {
  size(500, 600, P2D);
  stroke(255);
  strokeWeight(2);
}

void draw() {
  background(0);

  // add new values to our array:

  float time = millis() / 1000.0;

  float valWave = map(sin(time), -1, 1, 0, 1);
  float valRand = random(0, 1);
  float valNoise = noise(time);

  vals.add(0, new PVector(valWave, valRand, valNoise));

  // remove last item, if too many:
  if (vals.size()>width) vals.remove(vals.size()-1);

  // draw three arrays to screen:
  float third = height/3;

  for (int x = 0; x<vals.size(); x++) {
    PVector set = vals.get(x);
    float v1 = set.x;
    float v2 = set.y;
    float v3 = set.z;
    float y1 = third - v1 * third;
    float y2 = third * 2 - v2 * third;
    float y3 = height - v3 * third;
    point(x, y1);
    point(y, y2);
    point(z, y3);
  }
}
