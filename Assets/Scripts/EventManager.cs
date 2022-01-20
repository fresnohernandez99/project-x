using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

/*
    ** How to use it
    ** em.StartListening("test", new Action<string>(Function)); 
    ** PlayerPrefs.Save();
    **
    ** private void Function(string test) {
    **    Debug.Log(test);
    **}
    */

public class EventManager : Singleton<EventManager>
{

    public static Dictionary<string, Action<string>> eventDictionary;

    private void InitDict(){
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<string>>();
        }
    }

    public void StartListening(string eventName, Action<string> listener)
    {
        InitDict();
        Action<string> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void StopListening(string eventName, Action<string> listener)
    {
        InitDict();
        Action<string> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(string eventName, string eventParam)
    {
        InitDict();
        Action<string> thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
            // OR USE  instance.eventDictionary[eventName](eventParam);
        }
    }
}