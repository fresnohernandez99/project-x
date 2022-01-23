using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BattleGuest : MonoBehaviour
{
    public bool isNpc = false;
    private EventManager em = (EventManager)EventManager.Instance;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AskForBattle()
    {
        if (isNpc) AnswerYes();
        else
        {
            //em.
        }
    }

    public void AnswerYes() {
        em.TriggerEvent(EventManager.ACCEPT_BATTLE, "");
    }

    public void AnswerNo() {
        em.TriggerEvent(EventManager.DECLINE_BATTLE, "");
    }
}
