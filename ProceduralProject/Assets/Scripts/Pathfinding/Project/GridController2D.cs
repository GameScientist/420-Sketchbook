using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController2D : MonoBehaviour
{
    // A delegate is used within another function
    delegate Pathfinding.Node LookupDelegate(int x, int y);
    public static GridController2D _singleton;
    public static GridController2D singleton { get; private set; }
    private Transform[,] tiles = new Transform[12, 7];
    Pathfinding.Node[,] nodes;
    //private Vector2Int[] floorTiles = { new Vector2Int(9, 5), new Vector2Int(1, 1) };
    private Vector2Int leftFloor = new Vector2Int(1, 1);
    private Vector2Int rightFloor = new Vector2Int(9, 5);
    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null) // we already have a singleton...
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        //DontDestroyOnLoad(gameObject);
        foreach (Transform tile in GetComponentsInChildren<Transform>()) if (tile != transform && tile.GetComponent<PathfinderController>()==null) tiles[(int)tile.position.x, (int)tile.position.y] = tile;
    }

    // Update is called once per frame
    void Update() => MakeNodes();

    public void MakeNodes()
    {
        nodes = new Pathfinding.Node[12, 7];
        for (int x = 0; x < tiles.GetLength(0); x++) for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Pathfinding.Node n = new Pathfinding.Node();
                Vector3 pos = new Vector3(x, y, 0);

                n.pos = pos;
                TerrainTile terrain = tiles[x, y].GetComponent<TerrainTile>();
                if (terrain == null) n.moveCost = 144;
                else n.moveCost = terrain.floor ? 1 : 12;
                nodes[(int)pos.x, (int)pos.y] = n;
            }

        LookupDelegate lookup = (x, y) =>
        {
            if (x < 0) return null;
            if (y < 0) return null;
            if (x >= nodes.GetLength(0)) return null;
            if (y >= nodes.GetLength(1)) return null;
            return nodes[x, y];
        };

        // Lookup each neighbor within the nodes
        for (int x = 0; x < nodes.GetLength(0); x++) for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Pathfinding.Node n = nodes[x, y];

                Pathfinding.Node neighbor1 = lookup(x + 1, y);
                Pathfinding.Node neighbor2 = lookup(x - 1, y);
                Pathfinding.Node neighbor3 = lookup(x, y + 1);
                Pathfinding.Node neighbor4 = lookup(x, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);
            }
    }

    public Pathfinding.Node Lookup(Vector3 pos)
    {
        if (nodes == null) MakeNodes();
        float w = 1;
        float h = 1;

        //pos.x += w / 2;
        //pos.y += h / 2;
        print(pos);
        int x = (int)Mathf.Round(pos.x / w);
        int y = (int)Mathf.Round(pos.y / h);

        if (x < 0 || y < 0) return null;
        if (x >= nodes.GetLength(0) || y >= nodes.GetLength(1)) return null;

        return nodes[x, y];
    }

    public void ChangeLeftFloor(Vector2Int floor)
    {
        tiles[leftFloor.x, leftFloor.y].GetComponent<TerrainTile>().Toggle();
        leftFloor = floor;
    }

    public void ChangeRightFloor(Vector2Int floor)
    {
        tiles[rightFloor.x, rightFloor.y].GetComponent<TerrainTile>().Toggle();
        rightFloor = floor;
    }
}
