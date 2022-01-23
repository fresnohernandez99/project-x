using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;


public class OnlineExplorerPlayer : MonoBehaviour
{
    private PlayerSharedData data;
    public GameObject player;
    public GameObject playerName;
    public string PlayerNumber = "0";

    EventManager em = (EventManager)EventManager.Instance;

    public bool isListen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isListen)
        {
            em.StartListening("player_pos_" + PlayerNumber, new Action<string>(SetJsonData));
            isListen = true;
        }
        
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetJsonData(string json)
    {
        var onlinePlayerData = JsonUtility.FromJson<PlayerSharedData>(json);
        
        data = onlinePlayerData;
        playerName.GetComponent<Text>().text = data.playerName;

        //set position
        transform.position = new Vector3
            (
                data.position.x,
                data.position.y,
                transform.position.z
            );

        Debug.Log("works set json");
    }

    public void SetData(PlayerSharedData onlinePlayerData)
    {
        data = onlinePlayerData;
        playerName.GetComponent<Text>().text = data.playerName;

        //set position
        transform.position = new Vector3
            (
                data.position.x,
                data.position.y,
                transform.position.z
            );

        Debug.Log("works set data");
    }

    public string GetId()
    {
        return data.id;
    }
    
    public PlayerSharedData GetPlayerData() {
        return data;
    }
}
