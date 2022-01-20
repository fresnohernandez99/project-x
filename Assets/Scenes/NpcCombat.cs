using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class NpcCombat : MonoBehaviour {

    public GameObject timerText;

    private int timeSeconds = 0;
    private int actualTime = 0;
    private bool timeRunning = false;

    public GameObject player1Status;
    public GameObject player2Status;

    private PlayerCombatData playerCombatData;
    private PlayerCombatData npcCombatData;

    //Combat
    public static bool RoundRunning = false;


    void Start() {
        StartTimer(10);
    }

    void Update() {
        if (timeRunning) TimeOver();
    }

    //NPC
    public void SetNpcData()
    {

    }

    //PLAYER
    public void SetPlayerData()
    {
        //playerCombatData = 
    }

    //Combat
	public void StartBattle()
    {

    }

    public void FinishBattle()
    {

    }

    public void StartRound()
    {
        //Start round timer
        StartTimer(20);
        RoundRunning = false;
    }

    public void RoundTimeOver()
    {
        //Stoping round
        RoundRunning = false;
        CloseTimer();
        FinishRound();
    }

    public void FinishRound()
    {
        if( playerCombatData.playerLife <= 0 || npcCombatData.playerLife <= 0)
        {
            FinishBattle();
        } else
        {

        }
    }

    public void showResults()
    {
        //show top selected actack
        //show battle result on middle
        //restart to intial round position
    }

    


    //Timers
    public void StartTimer(int sec) {
        timeSeconds = sec;
        timeRunning = true;
        actualTime = 0;
        StartCoroutine(TimeRunning());
    }

    public void CloseTimer(){
        timeSeconds = 0;
        timeRunning = false;
        actualTime = 0;
        StopCoroutine(TimeRunning());
        timerText.GetComponent<Text>().text = "0";
    }

    private void TimeOver(){
        if (getTime() == 0) {
            if (RoundRunning)
            {
                RoundTimeOver();
            } 
        } else {
            timerText.GetComponent<Text>().text = getTime().ToString();
        }
    }
    
    public int getTime(){
		return timeSeconds - actualTime;
	}

    IEnumerator TimeRunning() {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(1);
        }
    }
    
    void timeCount() {
        actualTime += 1;
    }

}
