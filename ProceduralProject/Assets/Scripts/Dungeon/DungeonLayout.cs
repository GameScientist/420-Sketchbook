using UnityEngine;

/// <summary>
/// Define all types of rooms 
/// </summary>
public enum RoomType
{
    Void, // 0
    Normal, // 1
    RandomPOI, // 2
    Merchant, // 3
    Shrine, // 4
    QuestGiver, // 5
    Loot, // 6
    FloorEnter, // 7 
    FloorExit // 8
}


public class DungeonLayout
{
    private int smallPerLarge = 5;
    private int resolution = 0;
    private int hires = 0;

    private int[,] smallRooms;
    private int[,] bigRooms;

    public void Generate(int size)
    {
        resolution = size;
        hires = resolution * smallPerLarge;

        bigRooms = new int[resolution, resolution];
        smallRooms = new int[hires, hires];

        // Walk through rooms multiple times, defined by type of room
        WalkRooms(RoomType.FloorEnter, RoomType.FloorExit);
        WalkRooms(RoomType.RandomPOI, RoomType.RandomPOI);
        WalkRooms(RoomType.RandomPOI, RoomType.RandomPOI);
        WalkRooms(RoomType.RandomPOI, RoomType.RandomPOI);

        // Make big rooms
        MakeBigRooms();

        // Punch holes
    }

    private void WalkRooms(RoomType a, RoomType b)
    {
        // Starting room
        int x = Random.Range(0, hires);
        int y = Random.Range(0, hires);

        int half = hires / 2;

        // end rooms
        int endX = Random.Range(0, half);
        int endY = Random.Range(0, half);

        if (x < half) endX += half;
        if (y < half) endY += half;

        // Insert two rooms into dungeon
        SetSmallRoom(x, y, (int)a);
        SetSmallRoom(endX, endY, (int)b);


        // Walk until we reach the target room
        while (x != endX || y != endY)
        {
            int dir = Random.Range(0, 4); // 0 to 3
            int dis = Random.Range(2, 6); // 2 to 5

            int disX = endX - x;
            int disY = endY - y;

            // Make the position moved less random, or not random at all
            if (Random.Range(0, 100) < 50)
            {
                // Pick the direction which gets closer to the target
                if (Mathf.Abs(disX) > Mathf.Abs(disY))
                {
                    dir = (disX > 0) ? 3 : 2;
                }
                else
                {
                    dir = (disY > 0) ? 1 : 0;
                }
            }

            for (int i = 0; i < dis; i++)
            {
                if (dir == 0) y--;
                if (dir == 1) y++;
                if (dir == 2) x--;
                if (dir == 3) x++;

                x = Mathf.Clamp(x, 0, hires - 1);
                y = Mathf.Clamp(y, 0, hires - 1);

                if (GetSmallRoom(x, y) == 0)
                {
                    SetSmallRoom(x, y, 1);
                }
            } // for


        } // while
    } // function

    private void MakeBigRooms()
    {
        // Loop through each dimension of the array
        for (int x = 0; x < smallRooms.GetLength(0); x++)
        {
            for (int y = 0; y < smallRooms.GetLength(1); y++)
            {
                int val = GetSmallRoom(x, y);

                int xb = x / smallPerLarge;
                int yb = y / smallPerLarge;

                // If the value of big room is larger than that of little room
                if (GetBigRoom(xb, yb) < val)
                {
                    // Set them both equal
                    SetBigRoom(xb, yb, val);
                }
            }
        }
    }

    public int[,] GetRooms()
    {
        if (bigRooms == null)
        {
            Debug.LogError("GenerateData: Can't get rooms before calling Generate();");
            return new int[0, 0];
        }

        // Make a copy of the bigRooms array:
        int[,] copy = new int[bigRooms.GetLength(0), bigRooms.GetLength(1)];
        System.Array.Copy(bigRooms, 0, copy, 0, bigRooms.Length);

        // Return the copy of the bigRooms array
        return copy;
    }

    private int GetSmallRoom(int x, int y)
    {
        // Check to see if the room chosen is in the bounds of the array
        if (smallRooms == null) return 0;
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= smallRooms.GetLength(0)) return 0;
        if (y >= smallRooms.GetLength(1)) return 0;

        return smallRooms[x, y];
    }
    private void SetSmallRoom(int x, int y, int val)
    {
        // Check to see if the room chosen is in the bounds of the array
        if (smallRooms == null) return;
        if (x < 0) return;
        if (y < 0) return;
        if (x >= smallRooms.GetLength(0)) return;
        if (y >= smallRooms.GetLength(1)) return;

        smallRooms[x, y] = val;
    }
    private int GetBigRoom(int x, int y)
    {
        // Check to see if the room chosen is in the bounds of the array
        if (bigRooms == null) return 0;
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= bigRooms.GetLength(0)) return 0;
        if (y >= bigRooms.GetLength(1)) return 0;

        return bigRooms[x, y];
    }
    private void SetBigRoom(int x, int y, int val)
    {
        // Check to see if the room chosen is in the bounds of the array
        if (bigRooms == null) return;
        if (x < 0) return;
        if (y < 0) return;
        if (x >= bigRooms.GetLength(0)) return;
        if (y >= bigRooms.GetLength(1)) return;

        bigRooms[x, y] = val;
    }
}