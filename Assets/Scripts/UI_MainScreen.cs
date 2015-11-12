using UnityEngine;
using UnityEngine.UI;

public class UI_MainScreen : MonoBehaviour
{
    public Canvas canvas;
    public Button playButton;
    public Text highestScore;

	void Start ()
    {
        canvas = canvas.GetComponent<Canvas>();
        playButton = GetComponent<Button>();
        highestScore = GetComponent<Text>();
	}

    public void OnPlayPress()
    {
        Application.LoadLevel(1);
    }
	
	void Update ()
    {
	
	}
}
