using UnityEngine;
using System.Collections;

public class BallMover : MonoBehaviour
{
    bool ballChosen = false;
    Ball ballToMove;
    BallColor originalColor;

    Node startNode;
    Node finishNode;

    void Start()
    {        
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Grid.mouseNode.assignedBall)
            {
                if (!ballChosen)
                {
                    startNode = Grid.mouseNode;
                    ballChosen = true;
                    ballToMove = Grid.mouseNode.assignedBall;
                    colorFade(ballToMove, true);
                }
                else
                {
                    startNode = Grid.mouseNode;
                    colorFade(ballToMove, false);
                    ballToMove = Grid.mouseNode.assignedBall;
                    colorFade(ballToMove, true);
                }
            }

            else
            {
                if (ballChosen)
                {
                    finishNode = Grid.mouseNode;
                    MoveBall(ballToMove);
                    ballChosen = false;
                }
                else
                {
                    startNode = null;
                }
            }
        }
    }

    void colorFade(Ball ball, bool chosen)
    {
        if (chosen)
        {
            originalColor = ball.color;
            Renderer rend = ball.GetComponent<Renderer>();
            Material highlighted = Resources.Load("Black") as Material;

            rend.material = highlighted;
        }
        else
            ball.color = originalColor;
    }

    void MoveBall(Ball ball)
    {   
        Pathfinding.Instance.FindPath(startNode, finishNode);
        if (Grid.Instance.path.Count > 0)
        {
            startNode.walkable = true;
            startNode.assignedBall = null;
            MatchedBallsController.Instance.RemoveBallInfo(ball.color, startNode);

            finishNode.walkable = false;
            finishNode.assignedBall = ball; 

            ball.transform.position = finishNode.worldPosition;
            colorFade(ballToMove, false);
            MatchedBallsController.Instance.AddBallInfo(ball.color, finishNode);
        }
        else
        {
            colorFade(ballToMove, false);
        }
        Grid.Instance.path.Clear();
    }    
}
