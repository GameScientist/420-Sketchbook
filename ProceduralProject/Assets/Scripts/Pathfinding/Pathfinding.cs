using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to find the most desireable path to take towards a destination.
/// </summary>
public static class Pathfinding
{
    public class Node
    {
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;
        /// <summary>
        /// Measures the desireability of a path to be taken.
        /// The higher the number, the less desireable the path.
        /// </summary>
        public float moveCost = 1;
        public float x;
        public float y;
        public List<Node> neighbors = new List<Node>();
        public Node Parent { get; private set; }
        public Vector3 pos;
        public void UpdateParentAndG(Node parent, float extraG = 0)
        {
            Parent = parent;
            if (parent != null) G = parent.G + moveCost + extraG;
            else G = extraG;
        }
        /// <summary>
        /// Makes an educated guess as to how far we are from the end point.
        /// </summary>
        /// <param name="end">The end point.</param>
        public void DoHeuristic(Node end) => H = (end.pos - pos).magnitude;
    }

    /// <summary>
    /// Finds the most desireable path to take towards a destination.
    /// </summary>
    /// <param name="start">The beginning of the path.</param>
    /// <param name="end">The end of the path.</param>
    /// <returns>The most desireable path.</returns>
    public static List<Node> Solve(Node start, Node end)
    {
        if (start == null) return new List<Node>();

        List<Node> open = new List<Node>(); // all the nodes that have been discovered, but not "scanned"
        List<Node> closed = new List<Node>(); // these nodes are "scanned"

        start.UpdateParentAndG(null, 0);
        open.Add(start);

        // 1. travel from start to end
        while (open.Count > 0)
        {
            // find node in OPEN list with SMALLEST F value
            float bestF = 0;
            Node current = null;
            foreach (Node n in open)
            {
                if (n.F < bestF || current == null)
                {
                    current = n;
                    bestF = n.F;
                }
            }
            // if this node is the end, stop looping
            if (current == end) break;
            bool isDone = false;
            foreach (Node neighbor in current.neighbors) if (!closed.Contains(neighbor)) // node not in CLOSED
                {
                    if (!open.Contains(neighbor)) // node not in OPEN:
                    {
                        open.Add(neighbor);
                        float dis = (neighbor.pos - current.pos).magnitude;
                        neighbor.UpdateParentAndG(current, dis); // set child's 'parent' & 'G'
                        if (neighbor == end) isDone = true;
                        neighbor.DoHeuristic(end);
                    }
                    else // node already in OPEN:
                    {
                        float dis = (neighbor.pos - current.pos).magnitude;
                        if (current.G + neighbor.moveCost + dis < neighbor.G)
                        {
                            // it's shorter to move to neighbor from current
                            neighbor.UpdateParentAndG(current, dis);
                        }
                    }
                }
            closed.Add(current);
            open.Remove(current);
            if (isDone) break;
        }

        // 2. travel from end to start, building path
        List<Node> path = new List<Node>();
        for (Node temp = end; temp != null; temp = temp.Parent) path.Add(temp);


        // 3. reverse path
        path.Reverse();
        return path;
    }
}
/*
Dijkstra - 
    Keep a list of OPEN nodes
    Foreach node:
        Record how far node is from start.
        Add neighbors to OPEN list.
            If is END, return chain of nodes.
        Move to CLOSED list.

Greedy Best-Search - 
    Keep a list of OPEN nodes
    Pick one node most likely to be closer to END // Heuristic:
        Record how far node is from start.
        Add neighbors to OPEN list.
            If is END, return chain of nodes.
        Move to CLOSED list.

A* - 
    Keep a list of OPEN nodes
    Pick one node with lowest "cost" (cost = distance to start + distance to end).
        Add neighbors to OPEN list.
            Record how far node is from start.
            If is END, return chain of nodes.
        Move to CLOSED list.

    F = G + H
    Heurestic
        Euclidean (line to end)
        Manhattan (dx+dy)
 */