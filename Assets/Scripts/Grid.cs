using UnityEngine;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public Node[,] grid;
    public float nodeRadius;
    public Node mouseNode;    

    public List<Node> occupiedNodes;
    public List<Node> emptyNodes;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;
    Vector3 mousePosition;
    Vector3 worldBottomLeft;


    void Start()
    {
        emptyNodes = new List<Node>();
        occupiedNodes = new List<Node>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        RemapGrid();
    }

    public void RemapGrid()
    {
        List<Node> occupied = new List<Node>();
        List<Node> empty = new List<Node>();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                //
                if (grid[x, y].walkable)
                {
                    empty.Add(grid[x, y]);
                }
                else
                {
                    occupied.Add(grid[x, y]);
                }
            }
        }
        
        occupiedNodes = occupied;
        emptyNodes = empty;
    }

    void Update()
    {
        DetermineMousePosition();        
    }    

    void DetermineMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
        }
        mouseNode = NodeFromWorldPoint(mousePosition);
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, .1f, gridWorldSize.y));

        if (grid != null)
        {            
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (mouseNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, new Vector3(.9f, .1f, .9f));
            }
        }
    }
}
