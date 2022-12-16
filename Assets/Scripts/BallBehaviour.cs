using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    //Aqui ficam os tipos diferentes de bolas
    //Cada tipo tem comportamentos diferentes
    public enum BallType{
        enemy,
        friend,
        DarkerEnemy
    }

    [SerializeField]public BallType typeOfBall;

    public Vector3 movement;

    [Header("Variables for Green Friend")]
    public float greenBall_FixedSpeed=5f;
    public float points;

    [Header("Variables for Red Enemy")]
    public float redBall_FixedSpeed=5f;

    //velocidade CONSTANTE
    public float currentSpeed;
    public Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {        

        movement.x = Random.Range(-1f,1f);
        movement.y = Random.Range(-1f,1f);
        movement.z = 0;
    }

    // Update is called once per frame
    void Update()
    {

        //Se quisermos que o jogador se mova a uma velocidade diferente temos de escolher a bola correta para alterar     
        switch(typeOfBall)
        {
            case BallType.enemy:
                transform.position +=((movement.normalized) * (greenBall_FixedSpeed * Time.deltaTime));     
                break;
            case BallType.DarkerEnemy:
                transform.position +=((movement.normalized) * (greenBall_FixedSpeed * Time.deltaTime));     
                break;
            case BallType.friend:
                transform.position +=((movement.normalized) * (redBall_FixedSpeed * Time.deltaTime));   
                break;

        }   

        //Calculo da velocidade para ter a certeza que é fixa
        currentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        
    }

    private void FixedUpdate()
    {                  

    }
    private void OnCollisionEnter2D(Collision2D other) {
        //Aqui é verificado o embate com as paredes
        if(other.gameObject.CompareTag("SolidWalls"))
        {
            //Vetor de reflexão do embate
            if(typeOfBall!=BallType.DarkerEnemy)
            {
                movement = Vector3.Reflect(movement, other.GetContact(0).normal);
            }
            else//DarkerEnemy
            {
                var player = LevelManager.instance.player;

                //se o jogador estiver vivo ir na sua direção
                if(player!=null)
                {
                    transform.right = LevelManager.instance.player.position - transform.position;
                    movement = transform.right;
                }
                else
                {
                    movement = Vector3.Reflect(movement, other.GetContact(0).normal);
                }
                
            }


            movement = movement * 1.10f;
        }
        //embate com o jogador
        else if(other.gameObject.CompareTag("Player"))
        {
            if(typeOfBall == BallType.friend)
            {
                LevelManager.instance.AddScore(points,this.gameObject.transform);
                Destroy(this.gameObject);
            }
            else if (typeOfBall == BallType.enemy || typeOfBall == BallType.DarkerEnemy)
            {
                LevelManager.instance.KillThePlayer_GameIsOver();
            }

        }
    }
}
