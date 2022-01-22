using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using Random = UnityEngine.Random;

public class NpcCombat : MonoBehaviour {

    public GameObject timerText;

    private int timeSeconds = 0;
    private int actualTime = 0;
    private bool timeRunning = false;
    private bool isWaiting = false;

    public GameObject player1Status;
    public GameObject player2Status;

    public GameObject player1Name;
    public GameObject player2Name;

    public GameObject player1ActionImage;
    public GameObject player2ActionImage;

    public GameObject player1BattleAction;
    public GameObject player2BattleAction;

    public GameObject player1LifeSlider;
    public GameObject player2LifeSlider;

    public GameObject textActionP1;
    public GameObject textActionP2;

    public GameObject clock;
    public GameObject potionsCount;

    public GameObject actionBtn1;
    public GameObject actionBtn2;
    public GameObject actionBtn3;
    public GameObject actionBtn4;
    public GameObject actionsArea;

    public GameObject dialog;

    private Animator player1Animator;
    private Animator player2Animator;

    private EventManager em = (EventManager)EventManager.Instance;

    public Sprite[] actionSprites = new Sprite[4];

    public Sprite[] actionBattleP1 = new Sprite[4];
    public Sprite[] actionBattleP2 = new Sprite[4];
    public Sprite emptySprite;

    private Coroutine startBattleCoroutine;
    private Coroutine timeCoroutine;
    private Coroutine emulateBattleCoroutine;
    private Coroutine battleAnimationsCoroutine;

    public AudioSource hit;
    public AudioSource drink;
    public AudioSource claps;
    public AudioSource lose;
    public AudioSource music;
    public AudioSource ring;

    //Combat
    public static bool roundRunning = false;
    public static bool isHudAviable = true;

    string[] nameArray = new string[] { "Crico", "Zapo", "Scrugy", "Grogy" };

    void Start() {
        player1Animator = player1Status.GetComponent<Animator>();
        player2Animator = player2Status.GetComponent<Animator>();

        HudAviable(false);
        startBattleCoroutine = StartCoroutine(StartBattle());

        hit.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.sfx;
        drink.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.sfx;
        claps.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.sfx;
        lose.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.sfx;
        ring.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.sfx;
        music.GetComponent<AudioSource>().volume = EnviromentGameData.Instance.playerSavedData.music;

    }

    void Update() {
        if (timeRunning) TimeOver();
    }

    //NPC
    public void SetPlayersData()
    {
        var newEnemy = GenerateNpc();
        EnviromentGameData.Instance.playerCombatData.ResetForPlayerBattle(newEnemy);

        player1LifeSlider.GetComponent<Slider>().maxValue = EnviromentGameData.Instance.playerCombatData.playerLife;
        player2LifeSlider.GetComponent<Slider>().maxValue = EnviromentGameData.Instance.playerCombatData.enemy.playerLife;

        player1LifeSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerCombatData.playerLife;
        player2LifeSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerCombatData.enemy.playerLife;
    }

    public PlayerCombatData GenerateNpc()
    {
        var newEnemy = new PlayerCombatData();

        newEnemy.playerName = nameArray[Random.Range(0, 4)];

        if (EnviromentGameData.Instance.playerSavedData.playerLevel > 3)
        {
            newEnemy.playerLevel = Random.Range(
                EnviromentGameData.Instance.playerSavedData.playerLevel - 2,
                EnviromentGameData.Instance.playerSavedData.playerLevel + 2
            );
        } else
        {
            newEnemy.playerLevel = Random.Range(
                1,
                EnviromentGameData.Instance.playerSavedData.playerLevel + 2
            );
        }

        newEnemy.selectedClass = PlayerSavedData.CLASSES[
            Random.Range(
                0,
                 PlayerSavedData.CLASSES.Length
            )];

        return newEnemy;
    }

    //Combat Managers
    public IEnumerator StartBattle()
    {
        AskAndWait();
        SetPlayersData();
        //TODO modify visual
        player1Name.GetComponent<Text>().text = $"{EnviromentGameData.Instance.playerCombatData.playerName} ({EnviromentGameData.Instance.playerCombatData.playerLevel})";
        player2Name.GetComponent<Text>().text = $"{EnviromentGameData.Instance.playerCombatData.enemy.playerName} ({EnviromentGameData.Instance.playerCombatData.enemy.playerLevel})";
        potionsCount.GetComponent<Text>().text = $"{EnviromentGameData.Instance.playerCombatData.pi.healthPotions}";

        yield return new WaitForSeconds(3);

        StartRound();

        yield return null;
    }

    public void AskAndWait()
    {
        var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "DIALOG_ARE_YOU_READY");
        dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);
    }

    IEnumerator WinBattle()
    {
        Debug.Log("BATTLE OVER you win");
        var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "WIN_BATTLE");
        dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);

        LevelUp(true);
        SaveData();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(3);

        yield return null;
    }

    IEnumerator LoseBattle()
    {
        Debug.Log("BATTLE OVER you lose");
        var notificationMsg = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "LOSE_BATTLE");
        dialog.GetComponent<DialogAcceptCancel>().CreateNotification(notificationMsg);

        LevelUp(false);
        SaveData();

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(3);

        yield return null;
    }

    public void LevelUp(bool isWinner)
    {
        if (isWinner)
        {
            var exp = (int)EnviromentGameData.Instance.playerCombatData.enemy.playerLevel / 3;
            EnviromentGameData.Instance.playerSavedData.nextLevelPoints -= exp;
        } else
        {
            var exp = (int)EnviromentGameData.Instance.playerCombatData.enemy.playerLevel / 6;
            EnviromentGameData.Instance.playerSavedData.nextLevelPoints -= exp;
        }
        if (EnviromentGameData.Instance.playerSavedData.nextLevelPoints <= 0)
        {
            EnviromentGameData.Instance.playerSavedData.playerLevel++;
            EnviromentGameData.Instance.playerSavedData.nextLevelPoints = EnviromentGameData.Instance.playerSavedData.playerLevel * 70;
            EnviromentGameData.Instance.playerSavedData.playerLife += 30;
        }
    }

    public void SaveData()
    {
        EnviromentGameData.Instance.playerSavedData.pi = EnviromentGameData.Instance.playerCombatData.pi;
        SecurePlayerPrefs.SaveGameData();
    }

    public void StartRound()
    {
        StopCoroutine(startBattleCoroutine);
        //Start round timer
        Debug.Log("Start round");
        StartTimer(20);
        HudAviable(true);
    }

    public void RoundTimeOver()
    {
        //Stoping round
        HudAviable(false);
        CloseTimer();

        //seleccting default attack and wait for result
        if (!isWaiting)
        {
            EnviromentGameData.Instance.playerCombatData.actionSelected = 1;
            player1ActionImage.GetComponent<Image>().overrideSprite = actionSprites[0];
            isWaiting = true;
            player1Animator.SetBool("isWaiting", true);
            em.StartListening(EventManager.WAIT_FOR_RESULT, new Action<string>(RoundResult));
            emulateBattleCoroutine = StartCoroutine(EmulateBattle());
        }
        
    }

    public void FinishRound()
    {
        StopCoroutine(battleAnimationsCoroutine);

        Debug.Log(EnviromentGameData.Instance.playerCombatData.playerLife.ToString());
        Debug.Log(EnviromentGameData.Instance.playerCombatData.enemy.playerLife.ToString());

        if (EnviromentGameData.Instance.playerCombatData.playerLife - EnviromentGameData.Instance.playerCombatData.damageTaken <= 0)
        {
            //you lose
            StartCoroutine(LoseBattle());
        }
        else if(EnviromentGameData.Instance.playerCombatData.enemy.playerLife - EnviromentGameData.Instance.playerCombatData.enemy.damageTaken <= 0)
        {
            //you win
            StartCoroutine(WinBattle());
        } else
        {
            player1ActionImage.GetComponent<Image>().overrideSprite = null;
            player2ActionImage.GetComponent<Image>().overrideSprite = null;
            StartRound();
        }
    }

    //Combat Events
    public void SelectAction(int action) {
        if (action == 4 && EnviromentGameData.Instance.playerCombatData.pi.healthPotions < 1)
        {
            //not have potions
        } else
        {
            if (action == 4)
            {
                EnviromentGameData.Instance.playerCombatData.pi.healthPotions--;
                potionsCount.GetComponent<Text>().text = $"{EnviromentGameData.Instance.playerCombatData.pi.healthPotions}";
            }
            EnviromentGameData.Instance.playerCombatData.actionSelected = action;
            HudAviable(false);
            isWaiting = true;
            player1ActionImage.GetComponent<Image>().overrideSprite = actionSprites[action - 1];

            //wait for result
            player1Animator.SetBool("isWaiting", true);
            em.StartListening(EventManager.WAIT_FOR_RESULT, new Action<string>(RoundResult));

            emulateBattleCoroutine = StartCoroutine(EmulateBattle());
        }
    }

    public IEnumerator EmulateBattle()
    {
        //wait 3 seconds
        player2Animator.SetBool("isWaiting", true);

        var roundResult = new RoundResult();

        if (EnviromentGameData.Instance.playerCombatData.enemy.damageTaken
            > EnviromentGameData.Instance.playerCombatData.damageTaken)
        {
            EnviromentGameData.Instance.playerCombatData.enemy.actionSelected = Random.Range(1, 5);
        } else
        {
            EnviromentGameData.Instance.playerCombatData.enemy.actionSelected = Random.Range(1, 4);
        }

        //winner
        var confrontDataPlayer1 = roundResult.PlayerWins(
                EnviromentGameData.Instance.playerCombatData.actionSelected,
                EnviromentGameData.Instance.playerCombatData.enemy.actionSelected
        );

        var confrontDataPlayer2 = roundResult.PlayerWins(
                EnviromentGameData.Instance.playerCombatData.enemy.actionSelected,
                EnviromentGameData.Instance.playerCombatData.actionSelected
        );

        var isWinner = confrontDataPlayer1.win;

        Debug.Log($"resultado: {isWinner}");

        //damage
        var damageTaken = 0;
        var damageGiven = 0;

        //si los dos NO hacen miss
        if (!confrontDataPlayer1.wasMiss && !confrontDataPlayer2.wasMiss)
        {

            //not tied
            if (EnviromentGameData.Instance.playerCombatData.actionSelected != EnviromentGameData.Instance.playerCombatData.enemy.actionSelected)
            {
                //no gané
                if (!isWinner && !confrontDataPlayer2.wasMiss)
                {
                    confrontDataPlayer2 = roundResult.CalculateDamage(
                        confrontDataPlayer2,
                        EnviromentGameData.Instance.playerCombatData.enemy.actionSelected,
                        EnviromentGameData.Instance.playerCombatData.enemy
                    );

                    damageTaken = confrontDataPlayer2.damage;
                    Debug.Log($"Player 1 perdío: {damageTaken} vida");
                }
                //si gané
                else
                {
                    confrontDataPlayer1 = roundResult.CalculateDamage(
                        confrontDataPlayer1,
                        EnviromentGameData.Instance.playerCombatData.actionSelected,
                        EnviromentGameData.Instance.playerCombatData
                    );
                    damageTaken = confrontDataPlayer1.damage;
                    Debug.Log($"Player 1 quitó: {damageGiven} vida");
                }
            }
            //si es tied
            else
            {
                roundResult.wasTied = true;
                damageTaken = 5 + EnviromentGameData.Instance.playerCombatData.enemy.playerLevel;
                damageGiven = 5 + EnviromentGameData.Instance.playerCombatData.playerLevel;
                Debug.Log("Fue empate");
            }

            var lifeToHeal = 50;

            if (EnviromentGameData.Instance.playerCombatData.actionSelected == 4)
            {
                if (damageTaken > lifeToHeal)
                    damageTaken -= lifeToHeal;
                else damageTaken = 0;

                Debug.Log("Player 1 se curó");
            }

            if (EnviromentGameData.Instance.playerCombatData.enemy.actionSelected == 4)
            {
                if (damageGiven > lifeToHeal)
                    damageGiven -= lifeToHeal;
                else damageGiven = 0;

                Debug.Log("Player 2 se curó");
            }

            roundResult.isWinner = isWinner;
            roundResult.damageTaken = damageTaken;
            roundResult.damageGiven = damageGiven;
            roundResult.player1WasCritic = confrontDataPlayer1.wasCritic;
            roundResult.player2WasCritic = confrontDataPlayer2.wasCritic;

        }

        roundResult.enemyAction = EnviromentGameData.Instance.playerCombatData.enemy.actionSelected;
        roundResult.player1Miss = confrontDataPlayer1.wasMiss;
        roundResult.player2Miss = confrontDataPlayer2.wasMiss;


        yield return new WaitForSeconds(3);

        string json = JsonUtility.ToJson(roundResult);
        em.TriggerEvent(EventManager.WAIT_FOR_RESULT, json);

        yield return null;
    }

    public void RoundResult(string resultJson)
    {
        em.StopListening(EventManager.WAIT_FOR_RESULT, new Action<string>(RoundResult));
        StopCoroutine(emulateBattleCoroutine);
        var result = JsonUtility.FromJson<RoundResult>(resultJson);

        battleAnimationsCoroutine = StartCoroutine(BattelAnimations(result));

        CloseTimer();
    }

    //Execute result animations
    IEnumerator BattelAnimations(RoundResult result)
    {
        player2ActionImage.GetComponent<Image>().overrideSprite = actionSprites[result.enemyAction - 1];

        //animate out
        yield return new WaitForSeconds(2);
        player1Animator.SetBool("isWaiting", false);
        player2Animator.SetBool("isWaiting", false);

        player1Animator.SetBool("isGoingToBattle", true);
        player2Animator.SetBool("isGoingToBattle", true);

        //animate fight
        yield return new WaitForSeconds(2);
        showActionTexts(result);

        player1BattleAction.GetComponent<Image>().overrideSprite = actionBattleP1[EnviromentGameData.Instance.playerCombatData.actionSelected - 1];
        player2BattleAction.GetComponent<Image>().overrideSprite = actionBattleP2[result.enemyAction - 1];

        hit.GetComponent<AudioSource>().Play();

        if (EnviromentGameData.Instance.playerCombatData.actionSelected == 4 || EnviromentGameData.Instance.playerCombatData.enemy.actionSelected == 4)
            drink.GetComponent<AudioSource>().Play();


        //animate result
        yield return new WaitForSeconds(2);
        textActionP1.GetComponent<Text>().text = "";
        textActionP2.GetComponent<Text>().text = "";

        player1BattleAction.GetComponent<Image>().overrideSprite = emptySprite;
        player2BattleAction.GetComponent<Image>().overrideSprite = emptySprite;

        //player win
        player1Animator.SetBool("isGoingToBattle", false);
        player2Animator.SetBool("isGoingToBattle", false);

        //tied
        if (result.wasTied)
        {
            player1Animator.SetBool("isFinish", true);
            player2Animator.SetBool("isFinish", true);
        }
        //player win
        else if (result.isWinner)
        {
            player1Animator.SetBool("isFinish", true);
            player2Animator.SetBool("isGettingHit", true);
        }
        //npc win
        else
        {
            player1Animator.SetBool("isGettingHit", true);
            player2Animator.SetBool("isFinish", true);
        }

        updateLife(result.damageTaken, result.damageGiven);

        //reset round
        yield return new WaitForSeconds(1);
        player1Animator.Rebind();
        player1Animator.Update(0f);
        player2Animator.Rebind();
        player2Animator.Update(0f);

        FinishRound();

        yield return null;
    }

    //Animation
    public void showActionTexts(RoundResult result)
    {
        var critic = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "CRITIC");
        var miss = LocalizationSettings.StringDatabase.GetLocalizedString("GuiTexts", "MISS");

        if (result.player1WasCritic)
            textActionP1.GetComponent<Text>().text = critic;
        if (result.player1Miss)
            textActionP1.GetComponent<Text>().text = miss;

        if (result.player2WasCritic)
            textActionP2.GetComponent<Text>().text = critic;
        if (result.player2Miss)
            textActionP2.GetComponent<Text>().text = miss;

    }

    //Hud
    public void HudAviable(bool isAviable) {
        if (isHudAviable != isAviable)
        {
            if(isAviable)
            {
                actionBtn1.GetComponent<Button>().interactable = true;
                actionBtn2.GetComponent<Button>().interactable = true;
                actionBtn3.GetComponent<Button>().interactable = true;
                actionBtn4.GetComponent<Button>().interactable = true;
            }
            else
            {
                actionBtn1.GetComponent<Button>().interactable = false;
                actionBtn2.GetComponent<Button>().interactable = false;
                actionBtn3.GetComponent<Button>().interactable = false;
                actionBtn4.GetComponent<Button>().interactable = false;
            }
            isHudAviable = isAviable;
        }

        
    }

    public void updateLife(int damageToP1, int damageToP2)
    {
        EnviromentGameData.Instance.playerCombatData.damageTaken += damageToP1;
        EnviromentGameData.Instance.playerCombatData.enemy.damageTaken += damageToP2;

        player1LifeSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerCombatData.playerLife - EnviromentGameData.Instance.playerCombatData.damageTaken;
        player2LifeSlider.GetComponent<Slider>().value = EnviromentGameData.Instance.playerCombatData.enemy.playerLife - EnviromentGameData.Instance.playerCombatData.enemy.damageTaken;

        Debug.Log(
            $"vida de player 1 {player1LifeSlider.GetComponent<Slider>().value}  vida de player 2 {player2LifeSlider.GetComponent<Slider>().value}"
        );
    }

    //Timers
    public void StartTimer(int sec) {
        clock.GetComponent<Animator>().SetBool("isRinging", false);
        timerText.GetComponent<Text>().text = sec.ToString();
        timeSeconds = sec;
        timeRunning = true;
        roundRunning = true;
        actualTime = 0;
        timeCoroutine = StartCoroutine(RunTime());
    }

    public void CloseTimer(){
        roundRunning = false;
        timeRunning = false;
        actualTime = 0;
        StopCoroutine(timeCoroutine);
    }

    private void TimeOver(){
        if (getTime() == 0)
        {
            clock.GetComponent<Animator>().SetBool("isRinging", true);
            ring.GetComponent<AudioSource>().Play();
            if (roundRunning)
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

    IEnumerator RunTime() {
        while (true)
        {
            timeCount();
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }
    
    void timeCount() {
        actualTime += 1;
    }

}
