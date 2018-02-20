using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeechEvent {

    public string message;
    public float letterPause;
    public Emotion emotion;
    public float timeUntilNextSpeech;
    public SpeechReponse response = null;
    public string eventOnSpeechFinished;
}

[System.Serializable]
public class SpeechReponse
{
    public List<string> choicesEventID = new List<string>();
    public List<string> choices = new List<string>();
}
