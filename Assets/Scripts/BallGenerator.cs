using UnityEngine;
using System.Collections.Generic;

public class BallGenerator : MonoBehaviour
{
    public static BallGenerator Instance;
    public Ball ball;
    public Grid grid;
    int freeNodesTracker;

    public bool noMatches = false;

    void Awake()
    {
        grid.GetComponent<Grid>();
        Instance = this;
    }

    void Start()
    {
        Debug.Log("awake");
        GenerateBalls();
        noMatches = false;
    }
    
    void Update()
    {        
        if (noMatches)
        {
            GenerateBalls();
            noMatches = false;
        }
        if(freeNodesTracker <= 0)
        {
            Application.LoadLevel(2);
        }
    }

    void GenerateBalls()
    {
        
        for (int i = 0; i < 3; i++)
        {
            //random color and node values
            BallColor ballColor = (BallColor)Random.Range(0, 6);
            List<Node> emptyNodes = Grid.Instance.getEmptyNodes();
            freeNodesTracker = emptyNodes.Count;
            if (emptyNodes.Count > 0)
            {
                Node pos = emptyNodes[Random.Range(0, emptyNodes.Count)];
            
                //instantiate and assign ball to the node
                Ball newBall = (Ball)Instantiate(ball, pos.worldPosition, Quaternion.identity);
                newBall.color = ballColor;
                pos.assignedBall = newBall;

                //change empty and occupied nodes status
                Grid.Instance.UpdateNodeStatus(pos, false);

                //add info to the match controller
                MatchedBallsController.Instance.AddBallInfo(ballColor, pos);
            }
            freeNodesTracker--;
        }
    }
}
