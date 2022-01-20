using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerCombatData : GameData
{
    //Constants
    //status
    public static string SELECTING = "selecting";
    public static string WAITING = "waiting";
    //atacks
    public static int STONE = 1;
    public static int PAPER = 2;
    public static int SCISSORS = 3;


    public int playerLife = 100;

	public string playerName = "";

    public int playerLevel = 1;
    
    public PlayerInventary pi = null;

	public string selectedClass = "none_yet";

    public string playerStatus = SELECTING;

    public int playerAtackSelected = 0;

    public PlayerSharedData? enemy = null;

}
