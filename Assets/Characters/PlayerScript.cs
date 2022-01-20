using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D rb;
    public SpriteRenderer sprite; 
    public float speed = 6;
    public Animator anim;
    public GameObject rays;
    public GameObject hud;

    void Start() { 
        rb.freezeRotation = true;
    }

    void Update() 
    { 
        Move();

        rays.transform.Rotate(0, 0, Time.deltaTime * 6f);
    } 

    void Move() { 
        float x = Input.GetAxisRaw("Horizontal"); 
        float moveBy = x * speed; 
        rb.velocity = new Vector2(moveBy, rb.velocity.y); 

        if (x == -1){
            sprite.flipX = true;
        }
        if (x == 1){
            sprite.flipX = false;
        }

        if (x != 0) anim.SetBool("isWalking", true);
        else anim.SetBool("isWalking", false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag){
            case "Building":
                hud.GetComponent<GamePlayHud>().SetActionActive(true);
                break;
            case "BadGuy":
                hud.GetComponent<GamePlayHud>().SetAttackActive(true);
                break;
                
        }
        Debug.Log(col.gameObject.tag + " : " + gameObject.name + " : " + Time.time);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        switch(col.gameObject.tag){
            case "Building":
                hud.GetComponent<GamePlayHud>().SetActionActive(false);
                break;
            case "BadGuy":
                hud.GetComponent<GamePlayHud>().SetAttackActive(false);
                break;
                
        }
        Debug.Log(col.gameObject.tag + " : " + gameObject.name + " : " + Time.time);
    }
}
