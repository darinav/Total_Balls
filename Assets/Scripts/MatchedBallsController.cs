using UnityEngine;
using System.Collections.Generic;
using System;

enum line_type { col = 0, row };

public class MatchedBallsController : MonoBehaviour
{
    public static MatchedBallsController Instance;
    public Dictionary<BallColor, int[,]> MatchTracker;    

    void Awake()
    {
        Instance = this;

        int[,] rowsAndColumns = new int[2, 7];
        MatchTracker = new Dictionary<BallColor, int[,]>();
        Array values = Enum.GetValues(typeof(BallColor));
        foreach (BallColor color in values)
        {
            MatchTracker.Add(color, rowsAndColumns);
        }
    }

    public void AddBallInfo(BallColor color, Node node)
    {
        int[,] rowsAndColumns;
        MatchTracker.TryGetValue(color, out rowsAndColumns);
        int x = node.gridX;
        int y = node.gridY;

        rowsAndColumns[(int)line_type.row, y] += 1;
        rowsAndColumns[(int)line_type.col, x] += 1;

        if (rowsAndColumns[(int)line_type.col, x] >= 5 && rowsAndColumns[(int)line_type.row, y] >= 5
            && Grid.Instance.grid[x, y].assignedBall != null && Grid.Instance.grid[x, y].assignedBall.color.Equals(color))
        {
            if (rowsAndColumns[(int)line_type.col, x] >= 5)
                if (FindMatchedBalls(line_type.col, x, color))
                    rowsAndColumns[(int)line_type.row, y] += 1;

            if (!FindMatchedBalls(line_type.row, y, color))
                rowsAndColumns[(int)line_type.row, y] -= 1;
        }
        else
        {
            if (rowsAndColumns[(int)line_type.col, x] >= 5)
                FindMatchedBalls(line_type.col, x, color);

            if (rowsAndColumns[(int)line_type.row, y] >= 5)
                FindMatchedBalls(line_type.row, y, color);
        }
    }

    public void RemoveBallInfo(BallColor color, Node node)
    {
        int[,] rowsAndColumns;
        MatchTracker.TryGetValue(color, out rowsAndColumns);
        rowsAndColumns[(int)line_type.row, node.gridY] -= 1;
        rowsAndColumns[(int)line_type.col, node.gridX] -= 1;
    }

    Node get_element(line_type cols_priority, int first, int second)
    {
        if (cols_priority == (int)line_type.col)
        {
            return Grid.Instance.grid[first, second];
        }
        else
        {
            return Grid.Instance.grid[second, first];
        }
    }

    bool FindMatchedBalls(line_type rowOrColumnIdentifier, int line_index, BallColor color)
    {
        List<Node> nodesToClear = new List<Node>();
        bool removed = false;
        
            List<Node> columnNodesToRemove = new List<Node>();
            for (int i = 0; i < 7; i++)
            {
                Node current = get_element(rowOrColumnIdentifier, line_index, i);
                //Debug.Log("-matchedCounter1: " + matchedCounter);
                //Debug.Log("ballExistance:" + i + " " + Grid.Instance.grid[i, (int)rowOrColumn.y].assignedBall);
                if (current.assignedBall != null &&
                    current.assignedBall.color.Equals(color))
                {
                    columnNodesToRemove.Add(current);
                }
                else
                {
                    if (columnNodesToRemove.Count < 5)
                    {
                        //Debug.Log("1Clearing for i ==" + i);
                        columnNodesToRemove.Clear();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //Debug.Log("+matchedCounter1: " + matchedCounter);
            if (columnNodesToRemove.Count >= 5)
            {
                removed = true;
                foreach (Node node in columnNodesToRemove)
                {
                    nodesToClear.Add(node);
                }
            }

        //Debug.Log("nodes to clear: " + nodesToClear.Count + " " + rowOrColumn.x + " " + rowOrColumn.y);
        foreach (Node node in nodesToClear)
        {
            if (node.assignedBall)
            {
                node.assignedBall.DestroyBall();
                RemoveBallInfo(color, node);
                Grid.Instance.UpdateNodeStatus(node, true);
            }
        }

        return removed;
    }

}
