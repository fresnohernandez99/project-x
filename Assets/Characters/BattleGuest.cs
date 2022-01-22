using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGuest : MonoBehaviour
{
    public bool isNpc = false;
    private EventManager em = (EventManager)EventManager.Instance;

    public void AskForBattle()
    {
        if (isNpc) AnswerYes();
    }

    public void AnswerYes() {
        em.TriggerEvent(EventManager.ACCEPT_BATTLE, "");
    }

    public void AnswerNo() {
        em.TriggerEvent(EventManager.DECLINE_BATTLE, "");
    }
}
