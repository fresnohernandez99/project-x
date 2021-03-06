using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSharedData : GameData
{
    public string id = "";

	public string playerName = "";
	
    public int playerLevel = 1;

	public bool isFighting = false;
	
    public Vector2 position = new Vector2(-16.02F, 13F);
    
	public string selectedClass = "none_yet";

}
