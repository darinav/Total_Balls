using UnityEngine;
using System.Collections.Generic;
using System;

enum line_type { col = 0, row };

public class MatchedBallsController : MonoBehaviour
{
    public static MatchedBallsController Instance;
    public Dictionary<BallColor, int[,]> MatchTracker;
    public int totalScore;
    int streak;
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

    void AddScore(int numOfBalls)
    {
        if(numOfBalls == 0)
        {
            streak = 0;
            BallGenerator.Instance.noMatches = true;
        }
        else
        {
            totalScore += 2*numOfBalls;
            streak++;
            BallGenerator.Instance.noMatches = false;
        }

        if(streak > 1)
        {
            totalScore += 25*streak;
        }

        GlobalControlScript.Instance.SaveScore(totalScore);
    }

    public void AddBallInfo(BallColor color, Node node)
    {
        int[,] rowsAndColumns;
        MatchTracker.TryGetValue(color, out rowsAndColumns);
        int x = node.gridX;
        int y = node.gridY;

        rowsAndColumns[(int)line_type.row, y] += 1;
        rowsAndColumns[(int)line_type.col, x] += 1;
        
        int number_of_removed = 0;
        //check if there is a row and column simultaneously
        if (rowsAndColumns[(int)line_type.col, x] >= 5 && rowsAndColumns[(int)line_type.row, y] >= 5
            && Grid.Instance.grid[x, y].assignedBall != null && Grid.Instance.grid[x, y].assignedBall.color.Equals(color))
        {
            number_of_removed = FindMatchedBalls(line_type.col, x, color);
            if (number_of_removed != 0)
            {
                //adding a temporary ball so row can also be matched
                rowsAndColumns[(int)line_type.row, y] += 1;
            }
            AddScore(number_of_removed);

            int number_of_removed2 = FindMatchedBalls(line_type.row, y, color);
            if (number_of_removed2 == 0)
            { 
                //removing temporary ball
                rowsAndColumns[(int)line_type.row, y] -= 1;
            }
            
            AddScore(number_of_removed + number_of_removed2);
        }
        else
        {
            if (rowsAndColumns[(int)line_type.col, x] >= 5)
            {
                number_of_removed = FindMatchedBalls(line_type.col, x, color);
            }
            else if (rowsAndColumns[(int)line_type.row, y] >= 5)
            {
                number_of_removed = FindMatchedBalls(line_type.row, y, color);
            }

            AddScore(number_of_removed);
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

    int FindMatchedBalls(line_type rowOrColumnIdentifier, int line_index, BallColor color)
    {
        List<Node> columnNodesToRemove = new List<Node>();
        for (int i = 0; i < 7; i++)
        {
            Node current = get_element(rowOrColumnIdentifier, line_index, i);
            if (current.assignedBall != null &&
                current.assignedBall.color.Equals(color))
            {
                columnNodesToRemove.Add(current);
            }
            else
            {
                if (columnNodesToRemove.Count < 5)
                {
                    columnNodesToRemove.Clear();
                }
                else
                {
                    break;
                }
            }
        }

        int number_of_removed = 0;
        if (columnNodesToRemove.Count >= 5)
        {
            number_of_removed = columnNodesToRemove.Count;
            foreach (Node node in columnNodesToRemove)
            {
                if (node.assignedBall)
                {
                    node.assignedBall.DestroyBall();
                    RemoveBallInfo(color, node);
                    Grid.Instance.UpdateNodeStatus(node, true);
                }
            }
        }
        return number_of_removed;
    }
}
