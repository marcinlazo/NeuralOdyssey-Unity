using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozomuTesting : MonoBehaviour {

    public bool autoStart;
    public TestSpeech[] zbior;

	void Start ()
    {
        
        if (autoStart)
        {
            EventManager.instance.dialogueEvent += DialogueInvoker;
            foreach (TestSpeech test in zbior)
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = test.message,
                    emotion = test.emotion,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 2
                });
            }  

            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "Hello.",
                emotion = Emotion.confident,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2,
            });

            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "What is your name?",
                emotion = Emotion.normal,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2,
                response = new SpeechReponse()
                {
                    choices = 
                    {
                        "Hello, my name is <name>.",
                        "I'm not telling you anything.",
                        "..."
                    },
                    choicesEventID =
                    {
                        "Response1_1",
                        "Response1_2",
                        "Response1_3"
                    }
                }
            });
            AI_Window.instance.TryNextSpeechEvent();
        }
	}
    void DialogueInvoker(string methodID)
    {
        Invoke(methodID, 0);
    }

    void Response1_1()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Nice to meet you.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "What are you doing here?",
            emotion = Emotion.surprized3,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "Just chilling",
                    "I'm coming to get you",
                    "Actually I'm just testing a dialogue system"
                },
                choicesEventID =
                    {
                        "whatHere_1",
                        "whatHere_2",
                        "whatHere_3"
                    }
            }
        }, true);
    }
    void Response1_2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Well then why don't you just drop dead.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        }, true);
    }
    void Response1_3()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Ah. The sound of silence.",
            emotion = Emotion.annoyedSmile,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        }, true);
    }
    void whatHere_1()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Seems like a dull place to chill.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        }, true);
    }
    void whatHere_2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Good luck, moron",
            emotion = Emotion.sad,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        }, true);
    }
    void whatHere_3()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "I hope it's working",
            emotion = Emotion.happy,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        }, true);
    }

    [System.Serializable]
    public struct TestSpeech
    {
        public string message;
        public Emotion emotion;
    }
}
