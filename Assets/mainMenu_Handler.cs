using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mainMenu_Handler : MonoBehaviour
{   
    public TextMeshProUGUI StartSceneScore;

    [Header("PlayAgain Screne Variables")]
    public Transform newRecord;
    public Transform NoNewRecord;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.CurrentSceneName == "PlayAgain")
        {
            //Novo recorde
            if(GameManager.instance.TempScore>PlayerPrefs.GetFloat("TotalPoints"))
            {
                NoNewRecord.gameObject.SetActive(false);
                newRecord.gameObject.SetActive(true);

                newRecord.GetComponent<TextMeshProUGUI>().text = "You have a new record!"+GameManager.instance.TempScore+" Points";
                newRecord.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(Previous "+ PlayerPrefs.GetFloat("TotalPoints")+")";
                PlayerPrefs.SetFloat("TotalPoints",GameManager.instance.TempScore);
            }
            else//quando ná novo recorde
            {
                newRecord.gameObject.SetActive(false);
                NoNewRecord.gameObject.SetActive(true);

                NoNewRecord.GetChild(0).GetComponent<TextMeshProUGUI>().text ="Current record : "+ PlayerPrefs.GetFloat("TotalPoints")+ " points";
            }
            
        }
        else
        {
            StartSceneScore.text="Max Points : " +PlayerPrefs.GetFloat("TotalPoints").ToString();
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void methodToQuitGame()
    {
        GameManager.instance.QuitGame();
    }

    public void methodToLoadScene(string Name)
    {
        if(GameManager.instance.CurrentSceneName == "PlayAgain")
        {
            //reset do score
            GameManager.instance.TempScore=0;
        }
        GameManager.instance.LoadScene(Name);
    }
}
