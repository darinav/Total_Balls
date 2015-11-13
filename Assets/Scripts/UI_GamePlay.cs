using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlay : MonoBehaviour
{    
    public Canvas canvas;
    public Text scoreText;
    public Button restart;

    private float timeToReload = 7f;
    private float timer = 0f;

    void Awake()
    {
        canvas = canvas.GetComponent<Canvas>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        restart = GameObject.Find("Restart").GetComponent<Button>();
    }

    void Update()
    {
        scoreText.text = "Current score: "+ "\n" + GlobalControlScript.Instance.score.ToString();
    }

    public void OnRestartPress()
    {
        Application.LoadLevel(1);
    }

 }
