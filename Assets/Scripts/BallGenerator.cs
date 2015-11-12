using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BallGenerator : MonoBehaviour
{
    public Ball ball;
    public Grid grid;

    public bool turnMade;
    public bool noMatches;

    void Awake()
    {
        grid.GetComponent<Grid>();
    }
    
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.H))
        {
            GenerateBalls();
        }
    }

    void GenerateBalls()
    {
        //random color and node values
        BallColor ballColor = BallColor.Green; //(BallColor)Random.Range(0, 6);
        List<Node> emptyNodes = Grid.Instance.getEmptyNodes();
        Node pos = emptyNodes[Random.Range(0, emptyNodes.Count)];

        if (pos != null) { 
        //instantiate and assign ball to the node
        Ball newBall = (Ball)Instantiate(ball, pos.worldPosition, Quaternion.identity);
        newBall.color = ballColor;
        pos.assignedBall = newBall;

        //change empty and occupied nodes status
        Grid.Instance.UpdateNodeStatus(pos, false);

        //add info to the match controller
        MatchedBallsController.Instance.AddBallInfo(ballColor, pos);
        }
    }
}
