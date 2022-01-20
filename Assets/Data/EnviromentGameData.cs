using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentGameData : Singleton<EnviromentGameData>
{
    public PlayerCombatData playerCombatData = new PlayerCombatData();
    public PlayerSavedData playerSavedData = new PlayerSavedData();
    public PlayerSharedData playerSharedData  = new PlayerSharedData();

    public void UpdatePlayer(string newName, string selectedClass) {
        playerCombatData.playerName = newName;
        playerSavedData.playerName = newName;
        playerSharedData.playerName = newName;

        playerCombatData.selectedClass = selectedClass;
        playerSavedData.selectedClass = selectedClass;
        playerSharedData.selectedClass = selectedClass;
    }

    public void LoadInitialData(PlayerSavedData psd){
        if (psd != null){
            playerSavedData = psd;

            playerSharedData.playerName = psd.playerName;
            playerSharedData.playerLevel = psd.playerLevel;
            playerSharedData.selectedClass = psd.selectedClass;

            playerCombatData.playerName = psd.playerName;
            playerCombatData.playerLevel = psd.playerLevel;
            playerCombatData.selectedClass = psd.selectedClass;
            playerCombatData.playerLife = psd.playerLife;
            playerCombatData.money = psd.money;
            playerCombatData.nextLevelPoints = psd.nextLevelPoints;
            playerCombatData.pi = psd.pi;
        }
    }

    public void ResetData(){
        playerCombatData = new PlayerCombatData();
        playerSavedData = new PlayerSavedData();
        playerSharedData  = new PlayerSharedData();
    }
}
