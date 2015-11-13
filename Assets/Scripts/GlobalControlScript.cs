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
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
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
