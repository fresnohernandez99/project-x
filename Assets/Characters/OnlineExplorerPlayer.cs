using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OnlineExplorerPlayer : MonoBehaviour
{
    private PlayerSharedData data;
    public GameObject playerName;
    public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(PlayerSharedData onlinePlayerData) {
        data = onlinePlayerData;

        playerName.GetComponent<Text>().text = data.playerName;
    }

    public PlayerSharedData GetPlayerData() {
        return data;
    }
}
