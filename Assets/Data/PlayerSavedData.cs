using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerSavedData : GameData
{
    public const string WARRIOR = "warrior";
    public const string ARCHER = "archer";
    public const string THIEF = "thief";
    public static string[] CLASSES = new string[] { "warrior", "archer", "thief" };

    public bool isNoob = true;

    public string serverIp = "127.0.0.1";

    public int playerLife = 100;

    public int money = 10;

    public int nextLevelPoints = 100;

	public string playerName = "Player";

    public int playerLevel = 1;
    
    public float music = 0.2F;

	public float sfx = 0.2F;
	
    public PlayerInventary pi = new PlayerInventary();

	public string selectedClass = "none_yet";
}
