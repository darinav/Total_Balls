using UnityEngine;
using UnityEngine.UI;

public class UI_MainScreen : MonoBehaviour
{
    public Canvas canvas;
    public Button playButton;
    public Text highestScore;

	void Awake ()
    {
        canvas = canvas.GetComponent<Canvas>();
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        highestScore = GameObject.Find("HighScore").GetComponent<Text>();
        
    }

    void Start()
    {
        highestScore.text = "Highest Score: " + GlobalControlScript.Instance.highestScore.ToString();
    }

    public void OnPlayPress()
    {
        Application.LoadLevel(1);
    }
	
	void Update ()
    {
	
	}
}
