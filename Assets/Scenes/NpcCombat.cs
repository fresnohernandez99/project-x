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

    void Start() {
        StartTimer(10);
    }

    void Update() {
        if (timeRunning) TimeOver();
    }


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
            Debug.Log("Time is over");
            CloseTimer();
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
