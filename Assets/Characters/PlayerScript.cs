using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public float speed = 6;
    public Animator anim;
    public GameObject rays;
    public GameObject hud;

    public GameObject? actualEnemy;

    private EventManager em = (EventManager)EventManager.Instance;
    private NetworkManager nm = (NetworkManager)NetworkManager.Instance;

    void Start() {
        rb.freezeRotation = true;
        hud.GetComponent<GamePlayHud>().SetBuyActive(false);
        hud.GetComponent<GamePlayHud>().SetAttackActive(false);
        hud.GetComponent<GamePlayHud>().SetActionActive(false);
        StartCoroutine(SendPosition());
    }

    void Update()
    {
        Move();

        rays.transform.Rotate(0, 0, Time.deltaTime * 6f);
    }

    private IEnumerator SendPosition()
    {
        EnviromentGameData.Instance.playerSharedData.position.x = transform.position.x;
        EnviromentGameData.Instance.playerSharedData.position.y = transform.position.y;

        yield return new WaitForSeconds(5);
        nm.EmitPlayerUpdate();

        StartCoroutine(SendPosition());
        yield return null;

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
            case "Shop":
                hud.GetComponent<GamePlayHud>().SetBuyActive(true);
                break;
            case "BadGuy":
                hud.GetComponent<GamePlayHud>().SetAttackActive(true);

                em.StartListening(EventManager.ACCEPT_BATTLE, new Action<string>(BattleAccepted));
                em.StartListening(EventManager.DECLINE_BATTLE, new Action<string>(BattleDeclined));

                actualEnemy = col.gameObject;
                break;
            case "OnlinePlayer":
                hud.GetComponent<GamePlayHud>().SetAttackActive(true);

                em.StartListening(EventManager.ACCEPT_BATTLE, new Action<string>(BattleAccepted));
                em.StartListening(EventManager.DECLINE_BATTLE, new Action<string>(BattleDeclined));

                actualEnemy = col.gameObject;
                break;

        }

        Debug.Log(col.gameObject.tag + " : " + gameObject.name + " : " + Time.time);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        switch(col.gameObject.tag){
            case "Shop":
                hud.GetComponent<GamePlayHud>().SetBuyActive(false);
                break;
            case "BadGuy":
                hud.GetComponent<GamePlayHud>().SetAttackActive(false);

                em.StopListening(EventManager.ACCEPT_BATTLE, new Action<string>(BattleAccepted));
                em.StopListening(EventManager.DECLINE_BATTLE, new Action<string>(BattleDeclined));

                actualEnemy = null;
                break;
            case "OnlinePlayer":
                hud.GetComponent<GamePlayHud>().SetAttackActive(false);

                em.StartListening(EventManager.ACCEPT_BATTLE, new Action<string>(BattleAccepted));
                em.StartListening(EventManager.DECLINE_BATTLE, new Action<string>(BattleDeclined));

                actualEnemy = col.gameObject;
                break;
        }
    }

    public void AskForBattle()
    {
        if (actualEnemy != null)
        {
            actualEnemy.GetComponent<BattleGuest>().AskForBattle();
        }
    }


    public void BattleAccepted(string _noData)
    {
        //if is npc go npc battle scene else go multiplayer
        if (actualEnemy.GetComponent<BattleGuest>().isNpc)
            SceneManager.LoadScene(4);
    }

    public void BattleDeclined(string _noData)
    {
        Debug.Log("The battle was declined");
        em.StopListening(EventManager.ACCEPT_BATTLE, new Action<string>(BattleAccepted));
        em.StopListening(EventManager.DECLINE_BATTLE, new Action<string>(BattleDeclined));

        actualEnemy = null;
    }

}
