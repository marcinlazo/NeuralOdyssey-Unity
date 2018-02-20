using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI_Window : MonoBehaviour {

    public static AI_Window instance;
    public static bool FastTalk;

    [SerializeField] RawImage emotionImage;
    [SerializeField] Text chatName;
    [SerializeField] Text chatText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] textBlips;
    [SerializeField] GameObject ai_avatar;
    [SerializeField] GameObject responsePanel;
    [SerializeField] Text responseText;
    public RawImage[] worldMonitors;
    public SpeechEvent TestSpeech;
    StringKeys keys;

    Queue<SpeechEvent> speechQueue = new Queue<SpeechEvent>();
    bool isSpeaking;
    int changeCounter;

    string displayedText;
    Emotion displayedEmotion;

    private void Awake()
    {
        instance = this;
        keys = new StringKeys();
        audioSource.ignoreListenerVolume = true;
    }
    private void Update()
    {
        changeCounter--;
        if(changeCounter <= 0)
        {
            ChangeEmotion(displayedEmotion);
            changeCounter = ChangeCounterGenerator();
        }
    }

    public void SetChatName(string input)
    {
        chatName.text = input;
    }
    public void AddSpeechEvent(SpeechEvent speech, bool startQueue = false)
    {
        speechQueue.Enqueue(speech);
        if (startQueue) TryNextSpeechEvent();
    }
    public void TryNextSpeechEvent()
    {
        if(speechQueue.Count > 0 && !isSpeaking)
        {
            isSpeaking = true;
            StartSpeechEvent(speechQueue.Dequeue());
        }
    }
    void StartSpeechEvent(SpeechEvent speech)
    {
        speech.message = keys.ReplaceKeys(speech.message);
        if(speech.response != null)
        {
            for (int i = 0; i < speech.response.choices.Count; i++)
            {
                speech.response.choices[i] = keys.ReplaceKeys(speech.response.choices[i]);
            }
        }
        StartCoroutine(TypeText(chatText, speech));
    }
    void FinishedSpeechEvent(SpeechEvent speech)
    {
        EventManager.instance.FireEvent(speech.eventOnSpeechFinished);
        isSpeaking = false;
        TryNextSpeechEvent();
    }

    private IEnumerator TypeText(Text text, SpeechEvent speech)
    {
        responsePanel.SetActive(false);
        text.text = "";
        displayedEmotion = speech.emotion;
        for (int i = 0; i < speech.message.Length; i++)
        {
            if (speech.message[i] != '&')
            {
                text.text += speech.message[i];
                PlayTextBlip();
            }
			//& acts as a special character for longer pauses
            else if(!FastTalk) yield return new WaitForSeconds(speech.letterPause * 2);

            yield return 0;
            if (!FastTalk) yield return new WaitForSeconds(speech.letterPause);
            if (!FastTalk && (speech.message[i] == '.' || speech.message[i] == '!' || speech.message[i] == '?')) yield return new WaitForSeconds(speech.letterPause*2);
        }
        speech.message = text.text;
        if (speech.response == null)
        {
            yield return new WaitForSeconds(FastTalk ? 0.1f : speech.timeUntilNextSpeech);
            InGame_Interface.instance.AddLogText(chatName.text, speech.message);
            FinishedSpeechEvent(speech);
        }
        else
        {
            string responseContent = "";
            for(int i = 0; i < speech.response.choices.Count; i++)
            {
                responseContent += string.Format("{0}. {1}\n", i + 1, speech.response.choices[i]);
            }
            responseText.text = responseContent;
            responsePanel.SetActive(true);
            bool awaitingInput = true;
            //Await Input
            while (awaitingInput)
            {
                bool[] input = new bool[5];
                input[0] = Input.GetKeyDown(KeyCode.Alpha1);
                input[1] = Input.GetKeyDown(KeyCode.Alpha2);
                input[2] = Input.GetKeyDown(KeyCode.Alpha3);
                input[3] = Input.GetKeyDown(KeyCode.Alpha4);
                input[4] = Input.GetKeyDown(KeyCode.Alpha5);
                for (int i = 0; i < input.Length; i++)
                {
                    if(input[i] && speech.response.choices.Count >= i + 1)
                    {
                        awaitingInput = false;
                        InGame_Interface.instance.AddLogText(PlayerData.name, speech.response.choices[i]);
                        EventManager.instance.DialogueEvent(speech.response.choicesEventID[i]);
                        responsePanel.SetActive(false);
                        break;
                    }
                }

                yield return null;
            }            
            FinishedSpeechEvent(speech);
        }
    }
    void PlayTextBlip()
    {
        audioSource.PlayOneShot(textBlips[Random.Range(0, textBlips.Length)]);
    }
    void ChangeEmotion(Emotion emotion, bool final = false)
    {
        emotionImage.texture = final ? NozomuData.GetEmotionFinal(emotion) : NozomuData.GetEmotion(emotion);
        foreach(RawImage im in worldMonitors) im.texture = final ? NozomuData.GetEmotionFinal(emotion) : NozomuData.GetEmotion(emotion);
    }
    int ChangeCounterGenerator()
    {
        return Random.Range(5, 10);
    }

    public void ShowAvatar(bool state)
    {
        ai_avatar.SetActive(state);
    }
    /// <summary>
    /// Resets the entire dialogue and stops current speech.
    /// </summary>
    public void ResetQueue()
    {
        StopAllCoroutines();
        speechQueue.Clear();
        isSpeaking = false;
    }
}
public struct EmotionSheet
{
    public EmotionDisplay emotion;
    public Sprite[] emotionSprites;
}
public enum EmotionDisplay
{
    normal
}
