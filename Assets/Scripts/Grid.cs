using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public static Grid Instance;
    public LayerMask unwalkableMask;
    public LayerMask groundMask;
    public Vector2 gridWorldSize;
    public Node[,] grid;
    public float nodeRadius;
    public static Node mouseNode;    

    float nodeDiameter;
    public int gridSizeX;
    public int gridSizeY;
    Vector3 mousePosition;
    Vector3 worldBottomLeft;
    public List<Node> path;
    public List<Vector2> neighborNodesCoord;


    void Awake()
    {
        Instance = this;
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        neighborNodesCoord = new List<Vector2>();
        neighborNodesCoord.Add(new Vector2(0, 1));
        neighborNodesCoord.Add(new Vector2(0, -1));
        neighborNodesCoord.Add(new Vector2(-1, 0));
        neighborNodesCoord.Add(new Vector2(1, 0));

    }

    void Update()
    {
        DetermineMousePosition();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                Debug.Log(worldPoint);
            }
        }
    }

    public void UpdateNodeStatus(Node node, bool walkable)
    {
        node.walkable = walkable;
    }

    public List<Node> getEmptyNodes()
    {
        List<Node> emptyNodes = new List<Node>();
        for (int i = 0; i < gridWorldSize.x; i++)
        {
            for (int j = 0; j < gridWorldSize.y; j++)
            {
                if (grid[i,j].walkable)
                {
                    emptyNodes.Add(grid[i, j]);
                }
            }
        }
        return emptyNodes;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        foreach (Vector2 neighbour in neighborNodesCoord)
        {
            int checkX = node.gridX + (int)neighbour.x;
            int checkY = node.gridY + (int)neighbour.y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }
        
       

        //for (int x = -1; x <= 1; x++)
        //{
        //    for (int y = -1; y <= 1; y++)
        //    {
        //        if (x == 0 && y == 0)
        //            continue;

        //        int checkX = node.gridX + x;
        //        int checkY = node.gridY + y;

        //        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
        //        {
        //            neighbours.Add(grid[checkX, checkY]);
        //        }
        //    }
        //}

        return neighbours;
    }

    void DetermineMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawLine(mousePosition, Camera.main.transform.position);
        if (Physics.Raycast(ray, out hit, groundMask))
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
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.blue;
                    }
                }
                if (mouseNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, new Vector3(1f, .1f, 1f));
            }
        }
    }
}
