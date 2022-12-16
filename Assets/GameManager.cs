using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string CurrentSceneName;
    public static GameManager instance;
    public IEnumerator SecondLevel;
    public IEnumerator PlayAgainScene;

    public float Time4Level1 = 30;
    public float Time4Level2 = 30;

    public float TempScore=0;

    public bool GameHasEnded;

    // Start is called before the first frame update
    void Awake()
    {
        CurrentSceneName = SceneManager.GetActiveScene().name;


        if(instance==null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadScene(string SceneName)
    {
        //check se o jogador está morto ao mudar de cena

        if (!GameManager.instance.GameHasEnded)
        {
            var scene = SceneManager.LoadSceneAsync(SceneName);
            scene.allowSceneActivation = true;

            GameManager.instance.CurrentSceneName = SceneName;
            if (SceneName == "Level1" && SecondLevel == null)
            {


                SecondLevel = MoveToSecondLevel();

                StartCoroutine(SecondLevel);
            }
            else if (SceneName == "Level2" && PlayAgainScene == null)
            {
                PlayAgainScene = MoveToPlayAgainScene();
                StartCoroutine(PlayAgainScene);
            }

        }


    }



    public void StopSceneCoroutines()
    {
        SecondLevel=null;
        PlayAgainScene=null;
        StopAllCoroutines();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator MoveToSecondLevel()
    {
        yield return new WaitForSeconds(Time4Level1);

        CurrentSceneName="Level2";

        SecondLevel =null;
        LoadScene("Level2");       

    }

    IEnumerator MoveToPlayAgainScene()
    {
        yield return new WaitForSeconds(Time4Level2);

        CurrentSceneName="PlayAgain";

        PlayAgainScene =null;
        LoadScene("PlayAgain");
    }
}
