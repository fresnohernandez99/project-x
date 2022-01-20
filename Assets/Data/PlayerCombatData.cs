using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerCombatData : GameData
{
    public int playerLife = 100;

    public int money = 10;

    public int nextLevelPoints = 100;

	public string playerName = "";

    public int playerLevel = 1;
    
    public PlayerInventary pi = null;

	public string selectedClass = "none_yet";
}
