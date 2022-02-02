using UnityEngine;

public enum RoomType
{
    Void,       // 0
    Normal,     // 1
    RandomPOI,  // 2
    Merchant,   // 3
    Shrine,     // 4
    QuestGiver, // 5
    Loot,       // 6
    FloorEnter, // 7
    FloorExit   // 8
}

public class DungeonLayout
{
    int lilsPerBig = 5;
    int res = 0;
    int hiRes = 0;
    int[,] lilRooms;
    int[,] bigRooms;

    public void Generate(int size)
    {
        res = size;
        res = res * lilsPerBig;

        bigRooms = new int[res, res];
        lilRooms = new int[hiRes, hiRes];

        WalkRooms(RoomType.FloorEnter, RoomType.FloorExit);
        WalkRooms(RoomType.RandomPOI, RoomType.RandomPOI);
        WalkRooms(RoomType.FloorEnter, RoomType.FloorExit);

        MakeBigRooms();

        // walk()
        // walk()
        // walk()
        // makeBigRooms()
    }

    private void WalkRooms(RoomType a, RoomType b)
    {
        // starting room:
        int x = Random.Range(0, hiRes);
        int y = Random.Range(0, hiRes);

        int half = hiRes / 2;

        //end rooms:
        int tx = Random.Range(0, half);
        int ty = Random.Range(0, half);

        if (x < half) tx += half;
        if (y < half) ty += half;

        // insert two rooms into dungeon:
        SetLilRoom(x, y, (int)a);
        SetLilRoom(tx, ty, (int)b);

        int totalRooms = 0;

        // walk to target room:
        while (x != tx || y != ty)
        {
            if (totalRooms++ > 256) break;

            int dir = Random.Range(0, 4);
            int dis = Random.Range(2, 6);

            // step into next room:
            for (int i = 0; i < dis; i++)
            {
                if (dir == 0) y--;
                if (dir == 1) y++;
                if (dir == 2) x--;
                if (dir == 3) x++;

                x = Mathf.Clamp(x, 0, hiRes - 1);
                y = Mathf.Clamp(y, 0, hiRes - 1);

                if (GetLilRoom(x, y) == 0)
                {
                    SetLilRoom(x, y, 1);
                }
                SetLilRoom(x, y, 1);

            }// ends for
        } // ends while
    }//ends

    private void MakeBigRooms()
    {
        for (int x = 0; x < lilRooms.GetLength(0); x++)
        {
            for (int y = 0; y < lilRooms.GetLength(0); y++)
            {
                int val = GetLilRoom(x, y);

                int xb = x / lilsPerBig;
                int yb = y / lilsPerBig;
            }
        }
    }

    public int[,] GetRooms()
    {
        if (bigRooms == null)
        {
            Debug.LogError("DungeonLayout: Must call Generate() before calling GetRooms()");
            return new int[0, 0];
        }

        // make an empty array, same size:
        int[,] copy = new int[bigRooms.GetLength(0), bigRooms.GetLength(1)];

        // copy data to new array
        System.Array.Copy(bigRooms, 0, copy, 0, bigRooms.Length);

        // return the copy
        return bigRooms;
    }

    private int GetLilRoom(int x, int y)
    {
        if (lilRooms == null) return 0;
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= lilRooms.GetLength(0)) return 0;
        if (y >= lilRooms.GetLength(0)) return 0;

        return lilRooms[x, y];
    }

    private void SetLilRoom(int x, int y, int val)
    {
        if (lilRooms == null) return;
        if (x < 0) return;
        if (y < 0) return;
        if (x >= lilRooms.GetLength(0)) return;
        if (y >= lilRooms.GetLength(0)) return;

        lilRooms[x, y] = val;
    }

    private int GetBigRoom(int x, int y)
    {
        if (bigRooms == null) return 0;
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= bigRooms.GetLength(0)) return 0;
        if (y >= bigRooms.GetLength(0)) return 0;

        return lilRooms[x, y];
    }

    private void SetBigRoom(int x, int y, int val)
    {
        if (bigRooms == null) return;
        if (x < 0) return;
        if (y < 0) return;
        if (x >= bigRooms.GetLength(0)) return;
        if (y >= bigRooms.GetLength(0)) return;

        bigRooms[x, y] = val;
    }
}