using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    //encontrar posição do player
    public Transform player;
    //criar bola verde
    public GameObject pf_greenBall;
    public List<Transform> ListOfGreenBalls;
    //criar bola vermelha
    public GameObject pf_redBall;
    public List<Transform> ListOfRedBalls;

    //criar bola que segue o jogador
    public GameObject pf_DarkerRedBall;
    public List<Transform> ListOfDarkerRedBalls;
    //Power Up
    public GameObject pf_powerUp;

    public BoxCollider2D SpawnArea;

    public float levelScore;

    [HideInInspector] public float LevelTime;

    //check do power up
    public bool isShrinkActive;
  
    // Start is called before the first frame update
    void Awake()
    {
        

        instance=this;

        if(SceneManager.GetActiveScene().name == "Level1")
        {
            LevelTime= GameManager.instance.Time4Level1;
        }
        else
        {
            LevelTime= GameManager.instance.Time4Level2;
        }

        player = GameObject.Find("Player").transform;
        SpawnArea = GameObject.Find("SpawnArea").GetComponent<BoxCollider2D>();        

        CreateNewGreenBall(GetRandomPointInsideCollider(SpawnArea));
    }

    private void Start() {
        levelScore = GameManager.instance.TempScore;
        //spawnar o power up
        StartCoroutine(spawnPowerUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillThePlayer_GameIsOver()
    {   
        //prevenir mudança de cena
        GameManager.instance.StopSceneCoroutines();

        //parar os power ups
        StopAllCoroutines();

        UI_handler.instance.GetComponent<Timer>().timerIsRunning=false;
        player.GetComponent<Animator>().SetTrigger("Dead");
        UI_handler.instance.activateGameOver();
    }

    public void CreateNewGreenBall(Vector3 pos)
    {
        var greenBall = Instantiate(pf_greenBall,pos,Quaternion.identity);

        ListOfGreenBalls.Add(greenBall.transform);
    }

    public void CreateNewRedBall(Vector3 pos)
    {
        //50-50 das bolas
        var flipcoin = Random.Range(0,100);

        if(flipcoin>=50)
        {
            var RedBall = Instantiate(pf_redBall,pos,Quaternion.identity);

            if(isShrinkActive)
            {
                RedBall.transform.localScale = new Vector3(RedBall.transform.localScale.x/2,RedBall.transform.localScale.y/2,0);
            }

            ListOfRedBalls.Add(RedBall.transform);
        }
        else
        {
            var DarkerRedBall = Instantiate(pf_DarkerRedBall,pos,Quaternion.identity);

            if(isShrinkActive)
            {
                DarkerRedBall.transform.localScale = new Vector3(DarkerRedBall.transform.localScale.x/2,DarkerRedBall.transform.localScale.y/2,0);
            }

            ListOfDarkerRedBalls.Add(DarkerRedBall.transform);
        }
        
    }

    //adicionar score
    public void AddScore(float score,Transform ball)
    {
        GameManager.instance.TempScore += score;
        UI_handler.instance.updateScore();
        
        RemoveFromGreenBallList(ball);

        CreateNewGreenBall(GetFurthestPointInsideCollider(SpawnArea,player.position));


        CreateNewRedBall(GetRandomPointInsideCollider(SpawnArea));
    }

    public void RemoveFromGreenBallList(Transform ballToRemove)
    {
        ListOfGreenBalls.Remove(ballToRemove);
    }
    public void RemoveFromRedBallList(Transform ballToRemove)
    {
        ListOfRedBalls.Remove(ballToRemove);
    }

    public Vector3 GetRandomPointInsideCollider( BoxCollider2D boxCollider )
    {        
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(Random.Range( -extents.x, extents.x ),Random.Range( -extents.y, extents.y ));    
        return boxCollider.transform.TransformPoint( point );
    } 

    IEnumerator spawnPowerUp()
    {
        yield return new WaitForSeconds(10);

        Instantiate(pf_powerUp,GetRandomPointInsideCollider(SpawnArea),Quaternion.identity);
    }

    public void ActivateShrinkPowerUp()
    {

        isShrinkActive=true;

        foreach(Transform redBall in ListOfRedBalls)
        {
            var newScale = new Vector3(redBall.transform.localScale.x/2,redBall.transform.localScale.y/2,0);
            redBall.localScale=newScale;
        }

        foreach(Transform darkRedBall in ListOfDarkerRedBalls)
        {
            var newScale = new Vector3(darkRedBall.transform.localScale.x/2,darkRedBall.transform.localScale.y/2,0);
            darkRedBall.localScale=newScale;
        }
        
        StartCoroutine(DisactiveShrinkPower_Countdown());
    }

    IEnumerator DisactiveShrinkPower_Countdown()
    {
        yield return new WaitForSeconds(10);

        DisActivateShrinkPowerUp();
    }

    public void DisActivateShrinkPowerUp()
    {
        isShrinkActive=false;

        foreach(Transform redBall in ListOfRedBalls)
        {
            var newScale = new Vector3(redBall.transform.localScale.x*2,redBall.transform.localScale.y*2,0);
            redBall.localScale=newScale;
        }

        foreach(Transform greenBall in ListOfGreenBalls)
        {
            var newScale = new Vector3(greenBall.transform.localScale.x*2,greenBall.transform.localScale.y*2,0);
            greenBall.localScale=newScale;
        }

        foreach(Transform darkRedBall in ListOfDarkerRedBalls)
        {
            var newScale = new Vector3(darkRedBall.transform.localScale.x*2,darkRedBall.transform.localScale.y*2,0);
            darkRedBall.localScale=newScale;
        }
        
    }

    public Vector3 GetFurthestPointInsideCollider( BoxCollider2D boxCollider ,Vector3 PlayerCurrentPos)
    {        
        //encontrar os cantos
        Bounds boxBounds = boxCollider.bounds;


        var ListCorners = new List<Vector3>();

        ListCorners.Add(new Vector3 (boxBounds.center.x+ boxBounds.extents.x,boxBounds.center.y+ boxBounds.extents.y));
        ListCorners.Add(new Vector3 (boxBounds.center.x- boxBounds.extents.x,boxBounds.center.y+ boxBounds.extents.y));
        ListCorners.Add(new Vector3 (boxBounds.center.x- boxBounds.extents.x,boxBounds.center.y- boxBounds.extents.y));
        ListCorners.Add(new Vector3 (boxBounds.center.x+ boxBounds.extents.x,boxBounds.center.y- boxBounds.extents.y));

        

        int maxIndex = 0;
        float maxDistance = 0;

        Debug.Log("Size list" + ListCorners.Count);
        for (int i = 0; i< ListCorners.Count; i++)
        {
            
            float distanceToNode = Vector3.Distance(PlayerCurrentPos, ListCorners[i]);
            if(distanceToNode > maxDistance)
            {
                maxDistance = distanceToNode ;
                maxIndex = i ;
            }
        }       
                   
        return  ListCorners[maxIndex];
    } 

    
}
