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

    public GameObject actionBtn1;
    public GameObject actionBtn2;
    public GameObject actionBtn3;
    public GameObject actionBtn4;
    public GameObject actionsArea;

    public GameObject dialog;

    private Animator player1Animator;
    private Animator player2Animator;

    private PlayerCombatData playerCombatData;
    private PlayerCombatData npcCombatData;

    //Combat
    public static bool RoundRunning = false;
    public static bool isHudAviable = true;

    string[] nameArray = new string[] { "Crico", "Zapo", "Scrugy", "Grogy" };

    void Start() {
        player1Animator = player1Status.GetComponent<Animator>();
        player2Animator = player1Status.GetComponent<Animator>();
        //SetPlayersData();

        HudAviable(false, 0.001F);
        StartCoroutine(StartBattle());
    }

    void Update() {
        if (timeRunning) TimeOver();
    }

    //NPC
    public void SetPlayersData()
    {
        var newEnemy = GenerateNpc();
        EnviromentGameData.Instance.playerCombatData.ResetForPlayerBattle(newEnemy);
        playerCombatData = EnviromentGameData.Instance.playerCombatData;
    }

    public PlayerCombatData GenerateNpc()
    {
        var newEnemy = new PlayerCombatData();

        newEnemy.playerName = nameArray[Random.Range(0, 3)];
        newEnemy.playerLevel = Random.Range(
                EnviromentGameData.Instance.playerSavedData.playerLevel - 2,
                EnviromentGameData.Instance.playerSavedData.playerLevel + 2
            );
        newEnemy.selectedClass = PlayerSavedData.CLASSES[
            Random.Range(
                0,
                 PlayerSavedData.CLASSES.Length - 1
            )];

        return newEnemy;
    }

    //Combat Managers
    public IEnumerator StartBattle()
    {
        AskAndWait();
        SetPlayersData();
        //TODO modify visual

        yield return new WaitForSeconds(3);

        StartRound();

        yield return null;
    }

    public void AskAndWait()
    {
        var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "DIALOG_ARE_YOU_READY");
        dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);
    }

    public void FinishBattle()
    {

    }

    public void StartRound()
    {
        //Start round timer
        StartTimer(20);
        RoundRunning = true;
        HudAviable(true);
    }

    public void RoundTimeOver()
    {
        //Stoping round
        RoundRunning = false;
        HudAviable(false);
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

    public IEnumerator showResults()
    {
        //show top selected actack
        var playerAttack = playerCombatData.playerAttackSelected;
        var npcAttack = npcCombatData.playerAttackSelected;
        showAttacks(playerAttack, npcAttack);
        new WaitForSeconds(3);
        //show battle result on middle
        //restart to intial round position
        yield return null;
    }

    //Combat Events
    public void SelectAction(int action) {

    }

    //Animations
    public void showAttacks(int playerAttack, int npcAttack)
    {

        switch (playerAttack)
        {
            case PlayerCombatData.STONE:
                //play middle animation with current class
                switch (playerCombatData.selectedClass)
                {
                    case PlayerSavedData.WARRIOR:
                        break;
                    case PlayerSavedData.ARCHER:
                        break;
                    case PlayerSavedData.THIEF:
                        break;
                }
                break;
            case PlayerCombatData.PAPER:
                //play middle animation
                break;
            case PlayerCombatData.SCISSORS:
                //play middle animation
                break;
        }


        switch (npcAttack)
        {
            case PlayerCombatData.STONE:
                //play middle animation with current class
                switch (playerCombatData.enemy.selectedClass)
                {
                    case PlayerSavedData.WARRIOR:
                        break;
                    case PlayerSavedData.ARCHER:
                        break;
                    case PlayerSavedData.THIEF:
                        break;
                }
                break;
            case PlayerCombatData.PAPER:
                //play middle animation
                break;
            case PlayerCombatData.SCISSORS:
                //play middle animation
                break;
        }

    }

    IEnumerator LerpPosition(GameObject obj, float lerpDuration = 5, float startValue = 0, float endValue = 10 )
    {
        float timeElapsed = 0;
        float valueTolerp = 0;

        while (timeElapsed < lerpDuration)
        {
            obj.transform.position = new Vector3(
                obj.transform.position.x,
                Mathf.Lerp(
                    obj.transform.position.y,
                    endValue,
                    timeElapsed / lerpDuration
                ),
                obj.transform.position.z
            );
            timeElapsed += Time.deltaTime;
            
            yield return null;
        }

        obj.transform.position = new Vector3(
            obj.transform.position.x,
            endValue,
            obj.transform.position.z
        );
    }

    //Hud
    public void HudAviable(bool isAviable, float time = 3F) {
        if (isHudAviable != isAviable)
        {
            if(isAviable)
            {
                actionBtn1.GetComponent<Button>().interactable = true;
                actionBtn2.GetComponent<Button>().interactable = true;
                actionBtn3.GetComponent<Button>().interactable = true;
                actionBtn4.GetComponent<Button>().interactable = true;

                StartCoroutine(LerpPosition(actionsArea, time, actionsArea.transform.position.y, actionsArea.transform.position.y + 600));
            }
            else
            {
                var sccreenH = UnityEngine.Screen.height;

                actionBtn1.GetComponent<Button>().interactable = false;
                actionBtn2.GetComponent<Button>().interactable = false;
                actionBtn3.GetComponent<Button>().interactable = false;
                actionBtn4.GetComponent<Button>().interactable = false;

                StartCoroutine(LerpPosition(actionsArea, 3F, actionsArea.transform.position.y, actionsArea.transform.position.y - 600));
            }
            isHudAviable = isAviable;
        }

        
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
