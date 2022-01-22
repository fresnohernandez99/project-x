using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerCombatData : GameData
{
    //Constants
    //status
    public const string SELECTING = "selecting";
    public const string WAITING = "waiting";
    //atacks
    public const int STONE = 1;
    public const int PAPER = 2;
    public const int SCISSORS = 3;
    //result
    public const int NONE_YET = 0;
    public const int PLAYER = 1;
    public const int NPC = 2;


    public int playerLife = 100;

    public int damageTaken = 0;

    public string playerName = "";

    public int playerLevel = 1;
    
    public PlayerInventary pi = null;

	public string selectedClass = "none_yet";

    public string playerStatus = SELECTING;

    public int actionSelected = 0;

    public PlayerCombatData? enemy = null;

    public int roundResult = NONE_YET;

    public void ResetForPlayerBattle(PlayerCombatData newEnemy) {
        playerLife = EnviromentGameData.Instance.playerSavedData.playerLife;
        playerName = EnviromentGameData.Instance.playerSavedData.playerName;
        playerLevel = EnviromentGameData.Instance.playerSavedData.playerLevel;
        pi = EnviromentGameData.Instance.playerSavedData.pi;
        selectedClass = EnviromentGameData.Instance.playerSavedData.selectedClass;
        playerStatus = SELECTING;
        actionSelected = 0;
        enemy = newEnemy;
        roundResult = NONE_YET;

    }

}
