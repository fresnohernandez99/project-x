using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerSavedData : GameData
{
    public bool isNoob = true;

    public string serverIp = "127.0.0.1";

    public int playerLife = 100;

    public int money = 10;

    public int nextLevelPoints = 100;

	public string playerName = "Player";

    public int playerLevel = 1;
    
    public float music = 100.0F;

	public float sfx = 100.0F;
	
    public PlayerInventary pi = null;

	public string selectedClass = "none_yet";
}
