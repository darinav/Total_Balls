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
                if (ballChosen == false)
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
            #region comm
            //if (ballChosen)
            //{
            //    if (!Grid.mouseNode.assignedBall)
            //    {
            //        finishNode = Grid.mouseNode;
            //        MoveBall(ballToMove);
            //        ballChosen = false;
            //    }
            //    else
            //    {
            //        ballChosen = false;
            //    }
            //}

            //else
            //{
            //    startNode = null;
            //    finishNode = null;
            //}


            //if (!Grid.mouseNode.assignedBall && ballChosen)
            //{
            //    finishNode = Grid.mouseNode;
            //    MoveBall(ballToMove);
            //    ballChosen = false;
            //}

            //else
            //{
            //    ballChosen = false;
            //    startNode = null;
            //    finishNode = null;
            //}
            #endregion
        }
    }

    void colorFade(Ball ball, bool chosen)
    {
        if (chosen)
        {
            originalColor = ball.color;
            ball.color = BallColor.Purple;
        }
        else
            ball.color = originalColor;
    }

    void MoveBall(Ball ball)
    {   
        Pathfinding.Instance.FindPath(startNode, finishNode);
        if (Grid.Instance.path != null)
        {
            startNode.walkable = true;
            startNode.assignedBall = null;
            finishNode.walkable = false;
            finishNode.assignedBall = ball;

            ball.transform.position = finishNode.worldPosition;
            colorFade(ballToMove, false);
        }
    }    
}
