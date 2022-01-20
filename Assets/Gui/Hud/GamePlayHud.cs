using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamePlayHud : MonoBehaviour
{
    public GameObject attack;
    public GameObject attackPlayer;
    public GameObject action;
    public GameObject playerClass;
    public GameObject playerName;
    public GameObject itemCount;

    private bool goLeft = true;

    private EventManager em = (EventManager) EventManager.Instance;
    private NetworkManager nm = (NetworkManager) NetworkManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        attack.SetActive(false);
        attackPlayer.SetActive(false);
        action.SetActive(false);
        
        var actualName = EnviromentGameData.Instance.playerSavedData.playerName;
        playerName.GetComponent<Text>().text = actualName;

        var potionCount = EnviromentGameData.Instance.playerSavedData.pi.healthPotions.ToString();
        itemCount.GetComponent<Text>().text = potionCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackActive(bool active){
        attack.SetActive(active); 
    }

    public void SetAttackPlayerActive(bool active){
        attackPlayer.SetActive(active); 
    }

    public void SetActionActive(bool active){
        action.SetActive(active); 
    }
}
