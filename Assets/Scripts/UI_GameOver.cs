using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    public Canvas canvas;
    public Text scoreText;
    public Button playAgain;

    private float timeToReload = 7f;
    private float timer = 0f;

    void Start ()
    {
        canvas = canvas.GetComponent<Canvas>();
        scoreText = GetComponent<Text>();
        playAgain = GetComponent<Button>();
	}

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToReload)
        {
            OnPlayPress();
        }
    }

    public void OnPlayPress()
    {
        Application.LoadLevel(0);
    }
}
