class Dungeon {

  int roomSize = 10;
  int res = 50;
  int[][] rooms;

  int lilPerBig = 5;
  int lowres() {
    return res/lilPerBig;
  }
  int[][] bigrooms;

  Dungeon() {
    generate();
  }

  void setRoom(int x, int y, int t) {
    if (x < 0) return;
    if (y < 0) return;
    if (x >= rooms.length) return;
    if (y >= rooms[x].length) return;

    int temp = getRoom(x, y);

    if (temp < t) rooms[x][y] = t;
  }
  void setBigRoom(int x, int y, int t) {
    if (x < 0) return;
    if (y < 0) return;
    if (x >= bigrooms.length) return;
    if (y >= bigrooms[x].length) return;

    int temp = getRoom(x, y);

    if (temp < t) rooms[x][y] = t;
  }
  int getRoom(int x, int y) {
    // check to make sure x and y do not fall out of bounds
    if (x < 0) return 0;
    if (y < 0) return 0;
    if (x >= rooms.length) return 0;
    if (y >= rooms[x].length) return 0;
    return rooms[x][y];
  }
  int getBigRoom(int x, int y) {
    // check to make sure x and y do not fall out of bounds
    if (x < 0) return 0;
    if (y < 0) return 0;
    if (x >= bigrooms.length) return 0;
    if (y >= bigrooms[x].length) return 0;
    return bigrooms[x][y];
  }
  void generate() {
    rooms = new int[res][res];

    walkRooms(3, 4);
    walkRooms(2, 2);
    walkRooms(2, 2);
    walkRooms(2, 2);

    makeBigRooms();

    punchHoles();

    // TODO: check for islands
  }
  void punchHoles() {
    for (int x = 0; x<bigrooms.length; x++) {
      for (int y = 0; y<bigrooms.length; y++) {
        int val = getBigRoom(x, y);
        if (val<1) continue; // only consider rooms of value 1
        if(random(0,100)<25) continue; // Don't punch hole one-forth of the time.
        int[] neighbors = new int[8];

        neighbors[0] = getBigRoom(x, y-1);
        neighbors[1] = getBigRoom(x+1, y-1);
        neighbors[2] = getBigRoom(x+1, y);
        neighbors[3] = getBigRoom(x+1, y+1);
        neighbors[4] = getBigRoom(x, y+1);
        neighbors[5] = getBigRoom(x-1, y+1);
        neighbors[6] = getBigRoom(x-1, y);
        neighbors[7] = getBigRoom(x-1, y-1);

        boolean isSolid = neighbors[7]>0;
        int talley = 0;
        for (int n : neighbors) {
          boolean s = n>0;
          
          if(s!=isSolid) talley++;
          isSolid = s;
        }
        if(talley<=2){
          // safe to punch a hole?
          setBigRoom(x, y, 0);
        }
      }
    }
  }
  void makeBigRooms() {
    int r = lowres();
    bigrooms = new int[r][r];

    for (int x = 0; x<rooms.length; x++) {
      for (int y = 0; y<rooms.length; y++) {
        int val=getRoom(x, y);
        int val2 = bigrooms[x/lilPerBig][y/lilPerBig];

        if (val>val2) {bigrooms[x/lilPerBig][y/lilPerBig]=val;
        }
      }
    }
  }
  void walkRooms(int type1, int type2) {
    // walking algorithm
    int halfW = rooms.length / 2;
    int halfH = rooms[0].length / 2;

    int x = (int)random(0, rooms.length);
    int y = (int)random(0, rooms[x].length);

    int tx = (int)random(0, halfW);
    int ty = (int)random(0, halfH);

    if (x < halfW) tx += halfW; // if starting point on left, move end point to right
    if (y < halfH) ty += halfH; // if starting point on top, move to bottom

    setRoom(x, y, type1);
    setRoom(tx, ty, type2);

    while (x != tx || y != ty) {
      int dir = (int)random(0, 4); // 0 to 3 for 4 directions
      int dis = (int)random(1, 4); // 1 to 3


      if (random(0, 100) > 50) {
        int dx = tx - x;
        int dy = ty - y;

        // check if we are closer to the goal on x axis than the y
        if (abs(dx) < abs(dy)) {
          // check if the target is above us
          // ? asks if dy is less than 0
          dir = (dy < 0) ? 0 : 1;
        } else { // if we are closer on y than x
          dir = (dx < 0) ? 3 : 2;
        }
      }



      for (int i = 0; i < dis; i++) {
        switch(dir) {
        case 0:
          y--; // move north
          break;
        case 1:
          y++; // move south
          break;
        case 2:
          x++; // move east
          break;
        case 3:
          x--; // move west
          break;
        }
        x = constrain(x, 0, res -1);
        y = constrain(y, 0, res -1);
        setRoom(x, y, 1);
      }
    }
  }
  void draw() {
    noStroke();
    int px;
    px = roomSize;
    for (int x = 0; x < rooms.length; x++) {
      for (int y = 0; y < rooms[x].length; y++) {
        int val = rooms[x][y];
        if (val>0) {
          switch(val) {
          case 1:
            fill(255);
            break;
          case 2:
            fill(0, 255, 0);
            break;
          case 3:
            fill(255, 0, 0);
            break;
          case 4:
            fill(0, 0, 255);
            break;
          }
          rect(x * px, y * px, px, px);
        }
      }
    }

    px = roomSize*lilPerBig;
    for (int x = 0; x < rooms.length; x++) {
      for (int y = 0; y < rooms[x].length; y++) {
        int val = rooms[x][y];
        if (val>0) {
          switch(val) {
          case 1:
            fill(255);
            break;
          case 2:
            fill(0, 255, 0);
            break;
          case 3:
            fill(255, 0, 0);
            break;
          case 4:
            fill(0, 0, 255);
            break;
          }
          rect(x * px, y * px, px, px);
        }
      }
    }
  }
}
