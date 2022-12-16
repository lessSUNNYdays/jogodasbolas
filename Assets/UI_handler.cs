using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_handler : MonoBehaviour
{
    public static UI_handler instance;
    public GameObject GameOver;
    public TextMeshProUGUI Score;
    // Start is called before the first frame update
    void Start()
    {
        instance=this;
        updateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateGameOver()
    {
        GameOver.SetActive(true);
    }

    public void restartGame()
    {
        GameManager.instance.GameHasEnded=false;
        GameManager.instance.TempScore = 0;
        GameManager.instance.LoadScene("Level1");

    }

    public void updateScore()
    {
        Score.text = "Score : "+ GameManager.instance.TempScore.ToString();
    }
}
