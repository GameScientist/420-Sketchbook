using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
    // A delegate is used within another function
    delegate Pathfinding.Node LookupDelegate(int x, int y);
    public static DanceFloor singleton { get; private set; }

    private Panel[,] panels = new Panel[16,8];
    private Pathfinding.Node[,] nodes;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton != null) // we already have a singleton...
        {
            Destroy(gameObject);
            return;
        }

        singleton = this;
        DontDestroyOnLoad(gameObject);
        foreach (Panel panel in GetComponentsInChildren<Panel>()) panels[(int)panel.transform.localPosition.x, (int)panel.transform.localPosition.z] = panel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNodes()
    {
        nodes = new Pathfinding.Node[panels.GetLength(0), panels.GetLength(1)];

        for (int x = 0; x < panels.GetLength(0); x++) for (int y = 0; y < panels.GetLength(1); y++)
            {
                Pathfinding.Node n = new Pathfinding.Node();

                n.pos = panels[x, y].transform.position;

                // If the cube has a wall, movecost is really high
                if (panels[x, y].wall) n.moveCost = 10;
                else n.moveCost = 1;// - panels[x, y].brightness;

                nodes[x, y] = n;
            }

        // Anonymous function only exists within this function
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

                Pathfinding.Node neighbor5 = lookup(x + 1, y + 1);
                Pathfinding.Node neighbor6 = lookup(x - 1, y + 1);
                Pathfinding.Node neighbor7 = lookup(x + 1, y - 1);
                Pathfinding.Node neighbor8 = lookup(x - 1, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);
                if (neighbor5 != null) n.neighbors.Add(neighbor5);
                if (neighbor6 != null) n.neighbors.Add(neighbor6);
                if (neighbor7 != null) n.neighbors.Add(neighbor7);
                if (neighbor8 != null) n.neighbors.Add(neighbor8);
            }
        /*Pathfinding.Node start = Lookup(helperStart.position);
        Pathfinding.Node end = Lookup(helperEnd.position);

        List<Pathfinding.Node> path = Pathfinding.Solve(start, end);*/

    }
    public Pathfinding.Node Lookup(Vector3 pos)
    {
        if (nodes == null) MakeNodes();
        float w = 1;
        float h = 1;

        int x = (int)Mathf.Round(pos.x / w);
        int y = (int)Mathf.Round(pos.z / h);
        if (x < 0 || y < 0) return null;
        if (x >= nodes.GetLength(0) || y >= nodes.GetLength(1)) return null;

        return nodes[x, y];
    }
}
