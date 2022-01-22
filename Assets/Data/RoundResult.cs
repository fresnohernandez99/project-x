using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

[Serializable]
public class RoundResult
{
    public int enemyAction = 0;

    public bool isWinner = false;

    public int damageTaken = 0;

    public int damageGiven = 0;

    public bool wasTied = false;

    public bool player1WasCritic = false;

    public bool player2WasCritic = false;

    public bool player1Miss = false;

    public bool player2Miss = false;


    public bool[] lowProbability = new bool[] { true, true, true, true, true, true, true, false, true, true };
    public bool[] midProbability = new bool[] { true, true, true, false, true, true };
    public bool[] hightProbability = new bool[] { true, false, true };

    public ConfrontData PlayerWins(int player1Action, int player2Action, bool isNpc)
    {
        if (!isNpc) Debug.Log($"player1: {player1Action} player2: {player2Action}");
        var confrontData = new ConfrontData();

        if (player1Action == 2 && player2Action == 1)
        {
            var result = midProbability[Random.Range(0, 6)];
            if (!result) confrontData.wasMiss = true;
            confrontData.win = true;
            return confrontData;
        }
        if (player1Action == 3 && player2Action == 2)
        {
            var result = lowProbability[Random.Range(0, 10)];
            if (!result) confrontData.wasMiss = true;
            confrontData.win = true;
            return confrontData;
        }
        if (player1Action == 1 && player2Action == 3)
        {
            var result = hightProbability[Random.Range(0, 3)];
            if (!result) confrontData.wasMiss = true;
            confrontData.win = true;
            return confrontData;
        }
        return confrontData;
    }

    public ConfrontData CalculateDamage(ConfrontData actual, int actionHit, PlayerCombatData player)
    {
        var damage = 0;
        var isCritic = false;
        switch (actionHit - 1)
        {
            case 0:
                isCritic = lowProbability[Random.Range(0, 10)];
                damage = 10 * player.playerLevel;

                break;
            case 1:
                isCritic = midProbability[Random.Range(0, 6)];
                damage = 5 * player.playerLevel;
                break;
            case 2:
                isCritic = hightProbability[Random.Range(0, 3)];
                damage = 15 * player.playerLevel;
                break;
        }

        if (isCritic)
        {
            Debug.Log("Is critic");
            actual.wasCritic = true;
            damage  += Random.Range(
                   (damage * player.playerLevel) - ((damage * player.playerLevel) / 2),
                   (damage * player.playerLevel) * 2
                 );
        }
        Debug.Log($"danno calculado: {damage}");
        actual.damage = damage;

        return actual;
    }
}
