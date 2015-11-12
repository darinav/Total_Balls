using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    Grid grid;
    const int diagonalMove = 14;
    const int straightMove = 10;

    void Awake()
    {
        grid = GetComponent<Grid>();
        Instance = this;
    }

    public void FindPath(Node startNode, Node targetNode)
    {        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        #region iterating openset
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                Debug.Log("finder found");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
        #endregion
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        Debug.Log(grid.path.Count);
    }

    // Search for the least of X or Y length and calculate how many 
    // diagonal and straight steps we need to make to reach target node
    // http://i.imgur.com/1JVYufD.png
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return diagonalMove * dstY + straightMove * (dstX - dstY);
        }
        return diagonalMove * dstX + straightMove * (dstY - dstX);
    }

}
