using UnityEngine;
using System.Collections.Generic;

public class BallGenerator : MonoBehaviour
{
    public Ball ball;
    public Grid grid;

    bool turnMade;
    bool noMatches;

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
        BallColor ballColor = (BallColor)Random.Range(0, 6);
        Node pos = grid.emptyNodes[Random.Range(0, grid.emptyNodes.Count)];
        Ball newBall = (Ball)Instantiate(ball, new Vector3(pos.gridX, .3f, pos.gridY), Quaternion.identity);
        newBall.color = ballColor;
        grid.occupiedNodes.Add(pos);
        grid.emptyNodes.Remove(pos);
        grid.RemapGrid();
    }   
}
