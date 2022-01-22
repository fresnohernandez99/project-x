using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GamePlayHud : MonoBehaviour
{
    public GameObject attack;
    public GameObject action;
    public GameObject playerClass;
    public GameObject playerName;
    public GameObject itemCount;
    public GameObject actualColor;
    public GameObject actualColorIndicator; 

    public GameObject player;

    private bool goLeft = true;

    private EventManager em = (EventManager) EventManager.Instance;
    private NetworkManager nm = (NetworkManager) NetworkManager.Instance;
    // Start is called before the first frame update
    void Start()
    {
        attack.SetActive(false);
        action.SetActive(false);
        
        var actualName = EnviromentGameData.Instance.playerSavedData.playerName;
        var actualLevel = EnviromentGameData.Instance.playerSavedData.playerLevel;
        playerName.GetComponent<Text>().text = $"{actualName} ({actualLevel})";

        var potionCount = EnviromentGameData.Instance.playerSavedData.pi.healthPotions.ToString();
        itemCount.GetComponent<Text>().text = potionCount;

        var money = EnviromentGameData.Instance.playerSavedData.money;
        actualColor.GetComponent<Text>().text = money.ToString();

        if (money < 256)
            actualColorIndicator.GetComponent<Image>().color = new Color32((byte)money, 0, 0, 100);
        else if (money < 510)
            actualColorIndicator.GetComponent<Image>().color = new Color32(0, (byte)money, 0, 100);
        else
            actualColorIndicator.GetComponent<Image>().color = new Color32(0, 0, (byte)money, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAttackActive(bool active){
        attack.SetActive(active); 
    }

    public void SetActionActive(bool active){
        action.SetActive(active); 
    }

    public void Attack()
    {
        player.GetComponent<PlayerScript>().AskForBattle();
    }
}
