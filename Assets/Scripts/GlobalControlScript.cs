using UnityEngine;
using System.Collections;

public class GlobalControlScript : MonoBehaviour
{
    public static GlobalControlScript Instance;
    public int score;
    public int highestScore;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Debug.Log("BALLS");
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
            Debug.Log("deleted");
        }
        else
        {
            Debug.Log("Total balls");
        }
    }

    public void SaveScore(int newScore)
    {
        score = newScore;
        if(score > highestScore)
        {
            highestScore = score;
        }
    }
}
