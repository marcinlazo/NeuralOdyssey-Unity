using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AI_Window))]
public class AI_WindowEditor : Editor
{
    AI_Window window;

    void Awake()
    {
        window = ((AI_Window)target);
        if (window != null)
        {

        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Dopisz event", GUILayout.Width(200)))
        {
            window.AddSpeechEvent(window.TestSpeech, false);
        }
        if (GUILayout.Button("Dopisz event i odpal", GUILayout.Width(200)))
        {
            window.AddSpeechEvent(window.TestSpeech, true);
        }
    }
}
