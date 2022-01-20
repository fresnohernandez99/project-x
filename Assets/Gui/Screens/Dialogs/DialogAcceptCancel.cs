using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class DialogAcceptCancel : MonoBehaviour
{
    public GameObject menssage;
    public GameObject cancelBtn;
    public GameObject dialog;
    private EventManager em = (EventManager) EventManager.Instance;

    void Awake () {
        Close();
    }

    public void Create(string msg){
        cancelBtn.SetActive(true);
        menssage.GetComponent<Text>().text = msg;
        Open();
    }

    public void CreateNotification(string msg){
        cancelBtn.SetActive(false);
        menssage.GetComponent<Text>().text = msg;
        Open();
    }

    void Open(){
        dialog.SetActive(true);
    }

    void Close(){
        dialog.SetActive(false);
    }

    public void ClickAccept(){
        em.TriggerEvent(Constants.DIALOG_ACCEPT, "");
        Close();
    }

    public void ClickCancel(){
        em.TriggerEvent(Constants.DIALOG_CANCEL, "");
        Close();
    }
}
