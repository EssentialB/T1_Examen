using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public GameObject bala;
    public float velocity = 10;
    public float fuerzaSalto = 25;
    public LayerMask capaSuelo;
    public int saltosMax = 1;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    const int ANIMATION_RUN = 0;
    const int ANIMATION_JUMP = 1;
    const int ANIMATION_DEATH = 2;

    BoxCollider2D boxCollaider;
    private int saltosRestantes;

    bool vida=true;

    void Start()
    {       
        rb = GetComponent<Rigidbody2D>(); 
        sr = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>(); 
        boxCollaider = GetComponent<BoxCollider2D>();
        saltosRestantes = saltosMax;
    }

    void Update()
    {
        if(sr.flipX == false && Input.GetKeyUp(KeyCode.C)){ //disparar a la derecha
            var balaPosition = transform.position +  new Vector3(3, -1, 0);
            var gb = Instantiate(bala, balaPosition, Quaternion.identity) as GameObject;
            var controller = gb.GetComponent<BalaController>();
            controller.SetRightDirection();
        }     

        if(vida==true){
        rb.velocity = new Vector2(10, rb.velocity.y); 
        ChangeAnimation(ANIMATION_RUN); 
        }else
        if(vida==false){            
            rb.velocity = new Vector2(0, 0);
            ChangeAnimation(ANIMATION_DEATH);
        }
        
        if(EstaEnSuelo()){
            saltosRestantes = saltosMax;
        }

        if(Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0){
            rb.velocity = new Vector2(rb.velocity.x,0f);
            Saltar();
            ChangeAnimation(ANIMATION_JUMP);
            saltosRestantes--;
         
            }
    bool EstaEnSuelo(){
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollaider.bounds.center,new Vector2(boxCollaider.bounds.size.x , boxCollaider.bounds.size.y) ,0f,Vector2.down,0.2f,capaSuelo);
        return raycastHit.collider != null;
    }

    void Saltar(){
        rb.AddForce(new Vector2(0,fuerzaSalto), ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemigo")
        {
            vida=false;
        }
    }
    
    void ChangeAnimation(int animation){
        animator.SetInteger("Estado", animation);
    }
  }
}
