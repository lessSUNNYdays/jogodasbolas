using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Animator anim;
    public float movementSpeed =5f;

    public Rigidbody2D rig;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x>0 || movement.x<0 || movement.y>0 || movement.y<0)
        {

            anim.ResetTrigger("Idle");

            anim.SetTrigger("Moving");
        }
        else
        {
            anim.ResetTrigger("Moving");

            anim.SetTrigger("Idle");
        }
        
    }

    public void StopMovement()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled=false;
            //impede as bolas vermelhas de aplicar força ao player depois de morrer
        gameObject.GetComponent<BoxCollider2D>().enabled=false;
        rig.bodyType=RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate() {
        
        rig.MovePosition(rig.position + movement * movementSpeed * Time.fixedDeltaTime);        
    }

    public void killPlayer()
    {
        GameManager.instance.GameHasEnded = true;
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            Debug.Log("Power Uo");
            LevelManager.instance.ActivateShrinkPowerUp();

            Destroy(other.gameObject);
        }        
    }

}
