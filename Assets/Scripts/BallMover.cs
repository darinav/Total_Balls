using UnityEngine;
using System.Collections;

public class BallMover : MonoBehaviour
{
    bool ballChosen = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && Grid.mouseNode.assignedBall)
        {
            Debug.Log("ball found");
            Pathfinding.Instance.start = Grid.mouseNode;
            ballChosen = true;
        }
        if (Input.GetMouseButtonUp(0) && !Grid.mouseNode.assignedBall && ballChosen)
        {
            Debug.Log("finish found");
            Pathfinding.Instance.finish = Grid.mouseNode;
        }
    }

    void MoveBall(Ball ball, Node destination)
    {
        //Pathfinding.seeker = ball.transform;
        //Pathfinding.target.position = destination.worldPosition;
    }
    
}
