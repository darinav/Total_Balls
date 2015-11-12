using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public GameObject ballPrefab;
    //public Node coordinate;

    Renderer rend;
    BallColor _color;
    public BallColor color
        {
            get { return _color; }
            set
            {
                _color = value;
                Material mat = Resources.Load(color.ToString()) as Material;
                if (mat != null)
                {
                  rend.material = mat;
                }
            }
        }    

    void Awake()
    {
        rend = ballPrefab.GetComponent<Renderer>();        
    }

    public void DestroyBall()
    {
        Debug.Log("DestroyBall");
        Destroy(gameObject);
    }
}
