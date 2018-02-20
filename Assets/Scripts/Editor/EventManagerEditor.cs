using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventManager))]
public class EventManagerEditor : Editor
{
    EventManager window;
    string customEvent = "";
    void Awake()
    {
        window = ((EventManager)target);
        if (window != null)
        {

        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        customEvent = GUILayout.TextArea(customEvent);
        if (GUILayout.Button("Fire Event manually", GUILayout.Width(200)))
        {
            window.FireEvent(customEvent);
        }
    }
}
