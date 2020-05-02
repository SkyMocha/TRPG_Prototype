using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.Tile;

public class Pathfinding {

    private List<Node> openList;
    private HashSet<Node> closedList;

    private const int STRAIGHT_COST = 10, DIAGONAL_COST = 14;

    private int width, height;

    public Pathfinding () {
        width = Map.tiles.GetLength(0);
        height = Map.tiles.GetLength(1);
    }

    public List<Node> FindPath (Vector3 start, Vector3 end) {
        Map.setNodeMap();
       
        Node startNode = Map.GetNode(start);
        Node endNode = Map.GetNode(end);

        openList = new List<Node> { startNode };
        closedList = new HashSet<Node>();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.calcFCost();

        int iter = 0;

        while (openList.Count > 0) {
            Node currentNode = getLowestCost(openList);
            if (currentNode == endNode)
                return CalculatePath(endNode);

            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            //if (iter > 20) return null;

            foreach (Node neighbourNode in GetNeighbours(currentNode, endNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.walkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                //Debug.Log(neighbourNode + " " + tentativeGCost + " | " + neighbourNode.gCost);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.parent = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.calcFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

            iter++;

        }

        return null;
    }

    private void LogOpenSet ( ){
        foreach (Node x in openList)
            Debug.Log(x);
    }

    private List<Node> GetNeighbours(Node currentNode, Node end)
    {
        List<Node> neighbourList = new List<Node>();

        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(Map.GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(Map.GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < height) neighbourList.Add(Map.GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < width)
        {
            // Right
            neighbourList.Add(Map.GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(Map.GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < height) neighbourList.Add(Map.GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(Map.GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < height) neighbourList.Add(Map.GetNode(currentNode.x, currentNode.y + 1));

        // CALCULATE H COST
        foreach (Node node in neighbourList)
            node.hCost = CalculateDistanceCost(node, end);

        return neighbourList;
    }

    private List<Node> CalculatePath (Node endNode) {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    public int CalculateDistanceCost (Node a, Node b) {
        int xDist = Mathf.Abs(a.x - a.x);
        int yDist = Mathf.Abs(a.y - a.y);
        int remaining = Mathf.Abs(xDist - yDist);
        return DIAGONAL_COST * Mathf.Min(xDist, yDist) + STRAIGHT_COST * remaining;
    }

    private Node getLowestCost (List<Node> pathNodeList) {
        Node lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}

public class Node
{

    public int x, y;
    public int gCost, hCost, fCost;
    public bool walkable;

    public Node parent;

    public Node (int x, int y, bool walkable) {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
        gCost = 10000;
        calcFCost();
        parent = null;
    }

    public void calcFCost () {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + y + "\nhcost " + hCost + "    gcost" + gCost;
    }

    public Tile getTile () {
        return Map.tiles[x, y];
    }

}
