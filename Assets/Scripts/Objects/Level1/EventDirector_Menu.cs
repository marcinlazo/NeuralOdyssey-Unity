using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventDirector_Menu : MonoBehaviour {

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] textBlips;

    [SerializeField] Text[] RightText, LeftText;

    float letterPause = 0.01f;

    void Awake () {
        audioSource = GetComponent<AudioSource>();
	}
    void Start()
    {
        EventManager.instance.gameEvent += EventLauncher;
        EventManager.instance.dialogueEvent += EventLauncher;
    }
    void EventLauncher(string methodName)
    {
        Invoke(methodName, 0);
    }

    void StartIntro()
    {
        StartCoroutine(GameIntro());
    }
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] GameObject loadingPanel;
    IEnumerator GameIntro()
    {
        foreach (Text text in RightText) text.text = "";
        foreach (Text text in LeftText) text.text = "";
        menuPanel.SetActive(false);

        yield return new WaitForSeconds(2);
        dialogueCanvas.SetActive(true);

        yield return StartCoroutine(TypeText(RightText[0], string.Format("I have an offer for you, {0}.", PlayerData.name)));
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(TypeText(LeftText[0], "Go on, I'm available."));
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(TypeText(RightText[1], "The catch is you have to do it ASAP."));
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(TypeText(LeftText[1], "Not a problem. Tell me what I'm doing."));
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(TypeText(RightText[2], @"Robbing the government.
There's this military warehouse called The Archive and it just sent out a distress signal.
Get in, take the score, get out. The pay is top dollar."));
        yield return new WaitForSeconds(6);
        yield return StartCoroutine(TypeText(LeftText[2], "What's the take?"));
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(TypeText(RightText[3], "A diamond chip. Buyer says it's located in \"Vault 309\"."));
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(TypeText(LeftText[3], "Anything else?"));
        yield return new WaitForSeconds(3);

        yield return StartCoroutine(TypeText(RightText[4], @"Word of mouth says there's no personell or AI on site.
Supposed to be fully automated to eliminate human error in security.
Hurry. If the distress reaction team catches you there..."));
        yield return new WaitForSeconds(6);
        yield return StartCoroutine(TypeText(LeftText[4], ">connection terminated"));
        yield return new WaitForSeconds(6);

        loadingPanel.SetActive(true);
        yield return null;
        SceneManager.LoadScene("Level1");
    }
    private IEnumerator TypeText(Text text, string message)
    {
        text.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] != '&')
            {
                text.text += message[i];
                PlayTextBlip();
            }
            else yield return new WaitForSeconds(letterPause * 2);

            yield return 0;
            yield return new WaitForSeconds(letterPause);
            if (message[i] == '.' || message[i] == '!' || message[i] == '?') yield return new WaitForSeconds(letterPause * 2);
        }
        message = text.text;
    }
    void PlayTextBlip()
    {
        audioSource.PlayOneShot(textBlips[Random.Range(0, textBlips.Length)]);
    }
}
