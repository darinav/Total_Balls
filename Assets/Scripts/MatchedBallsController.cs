using UnityEngine;
using System.Collections.Generic;
using System;

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
        rowsAndColumns[0, node.gridX] += 1;
        rowsAndColumns[1, node.gridY] += 1;
        CheckMatches(color);
    }

    public void RemoveBallInfo(BallColor color, Node node)
    {
        int[,] rowsAndColumns;
        MatchTracker.TryGetValue(color, out rowsAndColumns);
        rowsAndColumns[0, node.gridX] -= 1;
        rowsAndColumns[1, node.gridY] -= 1;
    }

    void CheckMatches(BallColor color)
    {
        int[,] rowsAndColumns;
        MatchTracker.TryGetValue(color, out rowsAndColumns);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (rowsAndColumns[i, j] >= 5)
                {
                    //Debug.Log(i + " " + j + " " + rowsAndColumns[i, j]);
                    FindMatchedBalls(new Vector2(i, j), color);

                }
            }
        }        
    }

    void FindMatchedBalls(Vector2 rowOrColumn, BallColor color)
    {
        int matchedCounter = 0;
        List<Node> nodesToClear = new List<Node>();

        //checking column
        if (rowOrColumn.x == 1)
        {
            List<Node> columnNodesToRemove = new List<Node>();
            for (int i = 0; i < 7; i++)
            {
                //Debug.Log("-matchedCounter1: " + matchedCounter);
                //Debug.Log("ballExistance:" + i + " " + Grid.Instance.grid[i, (int)rowOrColumn.y].assignedBall);
                if (Grid.Instance.grid[i, (int)rowOrColumn.y].assignedBall != null &&
                    Grid.Instance.grid[i, (int)rowOrColumn.y].assignedBall.color == color)
                {
                    columnNodesToRemove.Add(Grid.Instance.grid[i, (int)rowOrColumn.y]);
                    matchedCounter++;
                }
                else
                {
                    if (matchedCounter < 5)
                    {
                        columnNodesToRemove.Clear();
                        matchedCounter = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("+matchedCounter1: " + matchedCounter);
            }
            if (matchedCounter >= 5)
            {
                foreach (Node node in columnNodesToRemove)
                {
                    nodesToClear.Add(node);
                }
            }
        }

        //checking row
        if (rowOrColumn.x == 0)
        {
            List<Node> rowNodesToRemove = new List<Node>();
            for (int i = 0; i < 7; i++)
            {
                //Debug.Log("-matchedCounter2: " + matchedCounter);
                //Debug.Log("ballExistance:" + i + " " + Grid.Instance.grid[(int)rowOrColumn.x, i].assignedBall);
                if (Grid.Instance.grid[(int)rowOrColumn.x, i].assignedBall != null &&
                    Grid.Instance.grid[(int)rowOrColumn.x, i].assignedBall.color == color)
                {
                    rowNodesToRemove.Add(Grid.Instance.grid[(int)rowOrColumn.x, i]);
                    matchedCounter++;
                }
                else
                {
                    if (matchedCounter < 5)
                    {
                        rowNodesToRemove.Clear();
                        matchedCounter = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("+matchedCounter2: " + matchedCounter);
            }
            if (matchedCounter >= 5)
            {
                foreach (Node node in rowNodesToRemove)
                {
                    nodesToClear.Add(node);
                }
            }
        }


        //Debug.Log("nodes to clear: " + nodesToClear.Count);
        foreach (Node node in nodesToClear)
        {
            if (node.assignedBall)
            {
                node.assignedBall.DestroyBall();
                RemoveBallInfo(color, node);
                Grid.Instance.UpdateNodeStatus(node, true);
            }
        }
    }

}
