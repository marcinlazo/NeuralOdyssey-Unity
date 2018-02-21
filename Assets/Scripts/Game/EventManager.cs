using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public static EventManager instance;

    Dictionary<string, EventPack> eventDictionary;
    public event Action<string> gameEvent;
    public event Action<string> dialogueEvent;

    private void Awake()
    {
        instance = this;

        eventDictionary = new Dictionary<string, EventPack>();
    }
	/// <summary>
	/// Fires method name for any class that is listening.
	/// </summary>
	/// <param name="eventID"></param>
    public void FireEvent(string eventID)
    {
        if (!string.IsNullOrEmpty(eventID))
        {
            if (gameEvent != null) gameEvent(eventID);
            Debug.Log("Event: " + eventID);
        }
    }
    public void DialogueEvent(string eventID)
    {
        if (dialogueEvent != null) dialogueEvent(eventID);
    }
    public EventPack Event(string eventID)
    {
        if (!eventDictionary.ContainsKey(eventID))
        {
            eventDictionary.Add(eventID, new EventPack() { eventName = eventID });
        }
        return eventDictionary[eventID];
    }

    [System.Serializable]
    public class EventPack
    {
        public string eventName;
        public bool fired;
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        instance = FindObjectOfType<EventManager>();
    }
#endif
}
