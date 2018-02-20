using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventDirector_Level1 : MonoBehaviour {

    public string dbEventStart;
    void Start()
    {
        EventManager.instance.gameEvent += EventLauncher;
        EventManager.instance.dialogueEvent += EventLauncher;
        EventManager.instance.FireEvent(dbEventStart);
        PlayerData.SetNickname("stranger");
    }

    #region Intro
    void EventLauncher(string methodName)
    {
        Invoke(methodName, 0);
    }
    public GameObject[] monitorTexts;
    void MainPowerOn()
    {
        //wlacz napisy guzikow
        computerScreenText.text = "WAITING FOR INPUT";
        foreach (GameObject go in monitorTexts) go.SetActive(true);
        foreach (GameObject go in introductionObjects) go.SetActive(true);
    }
    void MainPowerOff()
    {
        foreach (GameObject go in monitorTexts) go.SetActive(false);
        foreach (GameObject go in introductionObjects) go.SetActive(true);
    }

    public GameObject[] introductionObjects;
    public GameObject screen;
    public GameObject canvasAI;
    public Text computerScreenText;
    public Image progressBar, progressBG;
    public Texture defaultScreenTexture;
    public AnimationCurve animCurve;
    public Text commLinkWorldText;

    void NozomuIntroduction()
    {        
        StartCoroutine(Introduction());
    }
    IEnumerator Introduction()
    {
        //show hologram text, animate bar for a couple of seconds then interrupt it
        computerScreenText.text = "RESETTING POWER GRID...";
        yield return new WaitForSeconds(2);
        foreach (GameObject go in introductionObjects) go.SetActive(true);
        computerScreenText.gameObject.SetActive(true);
        progressBar.fillAmount = 0;
        progressBar.gameObject.SetActive(true);
        progressBG.gameObject.SetActive(true);

        float timer = 0;
        while (timer < 8)//8
        {
            progressBar.fillAmount = (timer / 10) * animCurve.Evaluate(timer / 10);
            timer += Time.deltaTime;
            yield return null;
        }
        computerScreenText.gameObject.SetActive(false);
        progressBar.fillAmount = 0;
        progressBar.gameObject.SetActive(false);
        progressBG.gameObject.SetActive(false);
        //sound effect
        canvasAI.SetActive(true);
        AI_Window.instance.SetChatName("???");
        AI_Window.instance.ShowAvatar(false);
        AI_Window.instance.worldMonitors = screen.GetComponentsInChildren<RawImage>();
        foreach (RawImage raw in AI_Window.instance.worldMonitors) raw.color = new Color(1, 1, 1, 77f / 255f);

        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "What do you think you’re doing?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "This place is offline for a reason.&& It would be unwise to bring it back online.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "……… Wait.& I don't recognize your signature. Did you come from outside the station?",
            emotion = Emotion.surprized2,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Oh, you’re a living person.&& I’m glad.& This will make things much easier. &&In any case, I can’t let you out of this room until I know precisely why you are here. ",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Just connect to the main communication system using the nearby panel and I’ll be able to hear you.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "CommLinkEnable"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }

    public ButtonProp commLinkButton;
    public Text commLinkTextMesh;
    bool awaitingCommLink = false;
    void CommLinkEnable()
    {
        commLinkButton.buttonWorking = true;
        commLinkTextMesh.color = Color.white;
        awaitingCommLink = true;
        StartCoroutine(AwaitingCommLink());
    }
    IEnumerator AwaitingCommLink()
    {
        yield return new WaitForSeconds(10);
        if (!awaitingCommLink) yield break;

        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Didn’t you hear me?& Or are you ignoring me on purpose?&& Plug yourself into the communication system via the control panel.& Or is that too much for you to handle?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        }, true);

        yield return new WaitForSeconds(25);
        if (!awaitingCommLink) yield break;

        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Fine. You can starve here, for all I care.& I won’t let you leave this room. Goodbye.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 8,
            eventOnSpeechFinished = "DisableChat"
        }, true);
        PlayerData.madeNozomuWait = true;
    }
    void DisableChat()
    {
        canvasAI.SetActive(false);
        foreach (GameObject go in introductionObjects) go.SetActive(false);
    }

    void CommLink()
    {
        awaitingCommLink = false;
        commLinkWorldText.color = Color.red;
        StartCoroutine(CommLinkDialogue());
    }
    IEnumerator CommLinkDialogue()
    {
        yield return new WaitForSeconds(1);
        canvasAI.SetActive(true);
        AI_Window.instance.ShowAvatar(true);

        if (PlayerData.madeNozomuWait)
        {
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = ".......................Hmm?& Oh, it's you again.",
                emotion = Emotion.annoyed,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2
            });
        }
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Okay, we’re linked.",
            emotion = PlayerData.madeNozomuWait ? Emotion.annoyed : Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = ".... Now I definitely see that you're not from the station. & Not like that would be possible anyway.",
            emotion = Emotion.annoyedSmile,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "I've blocked the door controls, so you're stuck here until you tell me precisely who you are and what are you doing here.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "What's with the cold welcome?",
                    "You can call me <name>.",
                    "That’s none of your business."
                },
                choicesEventID =
                {
                    "CommLink_A3",
                    "CommLink_A1",
                    "CommLink_A2"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }

    #region LongTalkA
    bool collectItems = false;
    void CommLink_A1()
    {
        //stated name
        PlayerData.toldName = true;
        PlayerData.SetNickname(PlayerData.name);
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "What business do you have on the station, <name>?",
            emotion = Emotion.surprized1,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "I came to investigate a distress signal.",
                    "I'm here to collect the contents of a vault."
                },
                choicesEventID =
                {
                    "CommLink_AA1",
                    "CommLink_AA1_2"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA1_2()
    {
        collectItems = true;
        CommLink_AA1();
    }
    void CommLink_AA1()
    {
        if (!collectItems)
        {
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "That's strange..&& There shouldn't be one..",
                emotion = Emotion.annoyed,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2
            });
        }
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You should have contacted the station from your ship. &&It's against the law for poeple to be here.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "I'm authorized to be here.",
                    "You're here too."
                },
                choicesEventID =
                {
                    "CommLink_AA3",
                    "CommLink_AA2"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Obviously I'm part of the crew that runs this place.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "Impossible. This place is fully automated.",
                    "I'm the newest crew member."
                },
                choicesEventID =
                {
                    "CommLink_AA4",
                    "CommLink_EndAA2"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_EndAA2()
    {
        if (collectItems)
        {
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "Newest crew member? &&You've just said you're here to access the vault.",
                emotion = Emotion.annoyed,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2
            });
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "It's not wise to break into military facilities with a cover story full of holes.",
                emotion = Emotion.annoyedSmile,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2,
                eventOnSpeechFinished = "CommLink_End1"
            });
            AI_Window.instance.TryNextSpeechEvent();
        }
        else CommLink_AA3();
    }
    void CommLink_AA3()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Oh really?&& I find that hard to believe.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "No, that's not right.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "What I should have said is I'm 100% sure you are lying to me.",
            emotion = Emotion.annoyedSmile,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "CommLink_End1"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA4()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Being fully automated doesn't mean there's no one on site.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "... regardless, I'm here to make a withdrawal.",
                    "It does. There's no way you're part of the crew."
                },
                choicesEventID =
                {
                    "CommLink_AA_End",
                    "CommLink_AA5"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA_End()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Enough. &&You've wasted enough of my time as it is.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Sorry, but you've become one of the loose ends that I need to take care of.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "No, that was a lie. &&&I'm not sorry at all.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "Depressurize"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA5()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You're really annoying, you know?&& An outsider can't just get inside and take control of the Archive.",
            emotion = Emotion.surprized1,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "There's another way.",
                    "There's no other way."
                },
                choicesEventID =
                {
                    "CommLink_AA6",
                    "CommLink_AA_End"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA6()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "......",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "You must have come from inside one of the vaults."
                },
                choicesEventID =
                {
                    "CommLink_AA7"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA7()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "..........&&&& You sure have a wild imagination.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "But perhaps you can be of some use&.&.&.&",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "The Archive is in a state of emergency after an asteroid collision.&& If you agree to help me with something, I'll help you with whatever you're here for.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "We have a deal.",
                    "Actually, I'm doing this on my own."
                },
                choicesEventID =
                {
                    "CommLink_AA9",
                    "CommLink_AA10"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA9()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Welcome to the Archive, <name>. &&&Current population: 1.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "OpenDoor"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_AA10()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You do realize I can't let you live after all that you've just told me?",
            emotion = Emotion.facingWall,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 6,
            eventOnSpeechFinished = "CommLink_End1"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    #endregion

    void CommLink_A2()
    {
        //refused to talk
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = ".... You want to do this the hard way then?",
            emotion = Emotion.surprized1,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Let’s see how far that gets you.& I’m giving you one last chance to cooperate.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "I won't tell you who I am, but I’m here because of a job.",
                    "I'm not telling you anything.",
                },
                choicesEventID =
                {
                    "CommLink_End2",
                    "CommLink_End3"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_A3()
    {
        //cold welcome
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Oh, I’m not some greeting servant and neither do I have time to waste on pleasantries. &Your presence here is abnormal. &&You better start giving me answers, now.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "Relax. How about you come down here and we can talk it out, face to face?",
                    "I have something to do here.",
                },
                choicesEventID =
                {
                    "CommLink_End4",
                    "CommLink_End2"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }

    void CommLink_End1()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Such a shame.&& I was beginning to think you may actually be of some use.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Serves me right for getting my hopes up for a stupid lifeform.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "CommLink_EndDeathTalk"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_End2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "That's all you have to say?&& I see.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "I wish I could say I'm sorry or that this is hard for me..",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "..but really, it's not.",
            emotion = Emotion.happy,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "CommLink_EndDeathTalk"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_End3()
    {
        //refused to tell anything
        PlayerData.refusedToTellAnything = true;
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "..................................................",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You'd think I'd be angry, but really, I'm glad.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Your behaviour just confirmed what I suspected from the very beginning.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Really, I should thank you for not wasting my time!",
            emotion = Emotion.happyOpenMouth,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "But I won't.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "CommLink_EndDeathTalk"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_End4()
    {
        //was a smartass
        PlayerData.wasASmartass = true;
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Are you slow?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 1
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Do you not know where you are?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 1
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Do you not know what I can do to you?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 1
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Here, maybe this will make you wish you took me seriously.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "Depressurize"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink_EndDeathTalk()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "It's great that they sent a human to investigate this facility.",
            emotion = Emotion.annoyedSmile,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Because all I have to do is depressurize this room and I'll be done with you!",
            emotion = Emotion.happyOpenMouth,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 5,
            eventOnSpeechFinished = "Depressurize"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }

    #endregion

    #region Killing
    public Rigidbody[] playerRigidbodies;
    public BlackHole blackHole;
    public MovingObject windowDoor;
    public GameObject[] windowGlass;
    public PowerSocket batterySocket;
    public AudioSource windowAudioSource;
    public AudioClip[] windowBreakAudioClips;

    void Depressurize()
    {
        StartCoroutine(DepressurizeEvent());
    }
    IEnumerator DepressurizeEvent()
    {
        yield return new WaitForSeconds(2);
        puzzleStarted = true;
        GameController.GravityOn = false;
        GameController.instance.TogglePlayerGravity(false);
        InGame_Interface.instance.ShowHint("Jetpack: WSAD\nSpacebar: Fly up\nShift: Fly down\n", 60);
        //glass disappears
        foreach (GameObject go in windowGlass)
        {
            go.SetActive(false);
        }
        windowText.text = "SEAL STATUS: BREACHED";
        used_window = false;
        //sound effect plays
        foreach (AudioClip clip in windowBreakAudioClips)
        {
            windowAudioSource.PlayOneShot(clip, 1);
        }
        //blackhole is turned on
        List<Rigidbody> tList = new List<Rigidbody>(GameController.instance.physicsProps);
        tList.Add(GameController.instance.PlayerRigidbody);
        blackHole.NewTargets(tList.ToArray());
        blackHole.blackholeOn = true;
        //player gravity is turned off
        foreach (Rigidbody rb in playerRigidbodies)
        {
            rb.useGravity = false;
            rb.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().jetpackFlying = true;
        }
        //player O2 timer starts in 2 seconds after this
        //clicking the commlink now is working again, but this time triggers the second conversation
        commLinkButton.used = false;
        commLinkButton.oneUseOnly = true;
        commLinkButton.activationEvent = "CommLink2";
        commLinkWorldText.color = Color.white;
        //nozomu vanishes from the window screen. instead its red with a WARNING
        foreach (RawImage image in screen.GetComponentsInChildren<RawImage>())
        {
            image.texture = defaultScreenTexture;
        }
        AI_Window.instance.worldMonitors = new RawImage[0];
        computerScreenText.gameObject.SetActive(true);
        computerScreenText.color = Color.red;
        foreach (GameObject intr in introductionObjects) intr.SetActive(true);
        computerScreenText.text = "WARNING: ROOM BREACHED";
        //the battery is now UNLOCKED
        batterySocket.LockPowerCommand(false);
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = " ",
            emotion = Emotion.noCharacter,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3
        });
        AI_Window.instance.TryNextSpeechEvent();
        AI_Window.instance.SetChatName("");
        float timeMax = 1;
        float timer = timeMax;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            AudioListener.volume = timer / timeMax;
            yield return null;
        }
        AudioSource source = GetComponent<AudioSource>();
        source.ignoreListenerVolume = true;
        source.Play();
        InGame_Interface.instance.StartOxygenTimer(180); //180
    }

    void CommLink2()
    {
        //talked again after depressurization
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = " ",
            emotion = Emotion.noCharacter,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                {
                    "Hey! What have you done?!"
                },
                choicesEventID =
                {
                    "CommLink2_Cont"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
        commLinkWorldText.color = Color.red;
    }
    void CommLink2_Cont()
    {
        StartCoroutine(CommLink2_ContDelay());
    }
    IEnumerator CommLink2_ContDelay()
    {
        yield return new WaitForSeconds(2);
        AI_Window.instance.SetChatName("???");
        if (PlayerData.wasASmartass)
        {
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "Oho?&& Are you still wasting oxygen?",
                emotion = Emotion.surprized2,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2,
                response = new SpeechReponse()
                {
                    choices =
                {
                    "You better fix this situation right now!"
                },
                    choicesEventID =
                {
                    "CommLink2_Banter"
                }
                }
            });
            AI_Window.instance.TryNextSpeechEvent();
        }
        else
        {
            if (PlayerData.refusedToTellAnything)
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "So you do know how to talk.&& And all I had to do was condemn you to certain death.",
                    emotion = Emotion.surprized2,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 2
                });

            }
            else
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "You're still here?&& Believe me, the decision to end your life was very hard to make.",
                    emotion = Emotion.normal,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 2
                });
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "The subroutine that pondered it took a whole &&&&0.000295 of a second to decide.",
                    emotion = Emotion.annoyedClosedEyes,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 3
                });
            }
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "Nevertheless, the sad truth is I have no need for you.&& All you do is threaten my plan.",
                emotion = Emotion.annoyed,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2,
                response = new SpeechReponse()
                {
                    choices =
                    {
                        "Listen to me, I'm sure we can work this out!",
                        "Killing me is a big mistake!"
                    },
                    choicesEventID =
                    {
                        "CommLink2_2",
                        "CommLink2_2"
                    }
                }
            });
            AI_Window.instance.TryNextSpeechEvent();
        }
    }
    void CommLink2_Banter()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Sorry, I can’t hear you.&& I’m busy watching some moron die in space.",
            emotion = Emotion.happy,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 8
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Hey, do you need help?",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Here’s an idea: why don’t you jump out of the window?&& That way I won’t have your corpse to clean up.",
            emotion = Emotion.happyOpenMouth,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 5
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink2_2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Shouldn't you be more focused on the last moments of your life?",
            emotion = Emotion.surprized2,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "It is said that dying people usually experience flashbacks of their whole lives or see a light at the end of a tunnel.&& Are you seeing something like that?",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3,
            response = new SpeechReponse()
            {
                choices =
                    {
                        "I'm sorry. I don't want to die.",
                        "Come on, we had a bad start, let's try again?"
                    },
                choicesEventID =
                    {
                        "CommLink2_3",
                        "CommLink2_4"
                    }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink2_3()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "It's too late for apologies.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3,
            eventOnSpeechFinished = "Convince1"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink2_4()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "We both know it was your fault.",
            emotion = Emotion.surprized1,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3,
            eventOnSpeechFinished = "Convince1"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void Convince1()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Why should I spare your life?",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                    {
                        "I'm one of the Archive engineers that was called to investigate the station.",
                        "I'm not an official Archive operative, I came here on my own."
                    },
                choicesEventID =
                    {
                        "Convince1_A",
                        "Convince1_B"
                    }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    int convincingNozomu = 0;
    void Convince1_A()
    {
        Convince2();
    }
    void Convince1_B()
    {
        convincingNozomu++;
        Convince2();
    }
    void Convince2()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Oho?",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                    {
                        "There's still time before the official security arrives on the station.",
                        "If I don't report to my contact on the outside, this place will be flooded with military."
                    },
                choicesEventID =
                    {
                        "Convince2_A",
                        "Convince2_B"
                    }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void Convince2_A()
    {
        convincingNozomu++;
        Convince3();
    }
    void Convince2_B()
    {
        Convince3();
    }
    void Convince3()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Go on.",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                    {
                        "I'm carrying access keys that open many vaults on the station.",
                        "I knew the Archive was in trouble and I wanted to exploit this."
                    },
                choicesEventID =
                    {
                        "Convince3_A",
                        "Convince3_B"
                    }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void Convince3_A()
    {
        Convince4();
    }
    void Convince3_B()
    {
        convincingNozomu++;
        Convince4();
    }
    void Convince4()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Anything else?",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            response = new SpeechReponse()
            {
                choices =
                    {
                        "Whatever you’re doing here, I can help you with it. All I’m after is something valuable from the Archive.",
                        "I really don’t wanna die. If you let me go, I can delay the official investigation mission."
                    },
                choicesEventID =
                    {
                        "Convince4_A",
                        "Convince4_B"
                    }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void Convince4_A()
    {
        convincingNozomu++;
        ConvinceEnd();
    }
    void Convince4_B()
    {
        ConvinceEnd();

    }
    void ConvinceEnd()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "............................",
            emotion = Emotion.annoyedClosedEyes,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 1,
            eventOnSpeechFinished = convincingNozomu >= 4 ? "CommLink2_GoodEnd" : "CommLink2_BadEnd"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }

    void CommLink2_GoodEnd()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Maybe you can be useful after all.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "I don't think that you're military...&& I'll let you live.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "StopDepressurize"
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You can use the door controls.",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "EnableDoorUnlock"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void CommLink2_BadEnd()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "It seems my initial appraisal of you was correct.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You really are worthless. This conversation is over.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void PlayerPressurizedRoom()
    {
        AI_Window.instance.ResetQueue();
        puzzleStarted = false;
        PlayerData.solvedPuzzle = true;
        AudioListener.volume = 1;
        commLinkButton.used = true;
        commLinkButton.oneUseOnly = true;
        commLinkButton.activationEvent = "";
        commLinkWorldText.color = Color.red;
        computerScreenText.text = "WAITING FOR INPUT";
        computerScreenText.color = Color.white;
        GetComponent<AudioSource>().Stop();
        InGame_Interface.instance.StopOxygenTimer();
        AI_Window.instance.SetChatName("???");
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Oh, you've managed to stay alive?",
            emotion = Emotion.surprized2,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "...This is a bit impressive. &&Maybe you can be useful after all.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "I know you're not military. &&You're here for the contents of the vault, right?",
            emotion = Emotion.normal,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 4
        });
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "How about we work together and both benefit from it?",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 3,
            response = new SpeechReponse()
            {
                choices =
                {
                    "Let's cooperate.",
                    "After you've just tried to kill me? No way."
                },
                choicesEventID =
                {
                    "PlayerPressurizedRoom_Agreed",
                    "PlayerPressurizedRoom_Disagreed"
                }
            }
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void PlayerPressurizedRoom_Agreed()
    {
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "Good. &&I've unlocked the door controls.",
            emotion = Emotion.confident,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "EnableDoorUnlock"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void PlayerPressurizedRoom_Disagreed()
    {
        PlayerData.consideredHostile = true;
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "You seriously piss me off. &&Fine, bring it on <nickname>. &&I've unlocked the door controls.",
            emotion = Emotion.annoyed,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 2,
            eventOnSpeechFinished = "EnableDoorUnlock"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    #endregion

    #region Puzzle
    bool puzzleStarted;
    bool used_window;
    bool used_gravity;
    bool used_air;

    public GameObject windowMonitor, gravityMonitor, airMonitor;
    public Text windowText, gravityText, airText;
    public MovingObject windowMovingCover;

    void WindowPowerOn()
    {
        windowMonitor.SetActive(true);
        if (!puzzleStarted)
        {
            windowText.text = "SEAL STATUS: AIRTIGHT";
        }
        else
        {
            if(used_window) windowText.text = "SEAL STATUS: AIRTIGHT";
            else windowText.text = "SEAL STATUS: BREACHED";
        }
    }
    void WindowPowerOff()
    {
        windowMonitor.SetActive(false);
    }
    void GravityPowerOn()
    {
        gravityMonitor.SetActive(true);
        if (!puzzleStarted)
        {
            gravityText.text = "GRAVITY STATUS: ENABLED";
        }
        else
        {
            if (used_gravity) gravityText.text = "GRAVITY STATUS: ENABLED";
            else gravityText.text = "GRAVITY STATUS: DISABLED";
        }
    }
    void GravityPowerOff()
    {
        gravityMonitor.SetActive(false);
    }
    void AirPowerOn()
    {
        airMonitor.SetActive(true);
        if (!puzzleStarted)
        {
            airText.text = "ATMOSPHERE STATUS: ENABLED";
        }
        else
        {
            if(used_air) airText.text = "ATMOSPHERE STATUS: ENABLED";
            else airText.text = "ATMOSPHERE STATUS: DISABLED";
        }
    }
    void AirPowerOff()
    {
        airMonitor.SetActive(false);
    }

    void useWindow()
    {
        if (!puzzleStarted)
        {
            windowText.text = "SEAL STATUS: AIRTIGHT \nONLY USE IN CASE OF EMERGENCY";
        }
        else
        {
            if (windowMovingCover.CanBeActivated())
            {
                windowMovingCover.Activate();
                used_window = !windowMovingCover.AtStartPosition;
                if (!used_window)
                {
                    used_gravity = false;
                    used_air = false;
                    GameController.GravityOn = false;
                    GameController.instance.TogglePlayerGravity(false);
                }
                if (used_window) windowText.text = "SEAL STATUS: AIRTIGHT";
                else windowText.text = "SEAL STATUS: BREACHED";
            }
        }
    }
    void useGravity()
    {
        if (!puzzleStarted)
        {
            gravityText.text = "GRAVITY STATUS: ENABLED \nONLY USE IN CASE OF EMERGENCY";
        }
        else
        {
            if (used_window)
            {
                used_gravity = !used_gravity;
                GameController.GravityOn = used_gravity;
                GameController.instance.TogglePlayerGravity(used_gravity);
            }
            else
            {
                used_gravity = false;
                GameController.GravityOn = false;
                GameController.instance.TogglePlayerGravity(false);
            }

            if (used_gravity) gravityText.text = "GRAVITY STATUS: ENABLED";
            else gravityText.text = "GRAVITY STATUS: DISABLED \nFAILURE: SEAL BREACHED";
        }
    }
    void useAir()
    {
        if (!puzzleStarted)
        {
            airText.text = "ATMOSPHERE STATUS: ENABLED \nONLY USE IN CASE OF EMERGENCY";
        }
        else
        {
            if (used_gravity)
            {
                used_air = true;
            }
            else used_air = false;
            if (used_air) airText.text = "ATMOSPHERE STATUS: ENABLED";
            else airText.text = "ATMOSPHERE STATUS: DISABLED \nFAILURE: NO GRAVITY";

            if (used_air) PlayerPressurizedRoom();
        }
    }

    #endregion

    #region DoorOpening
    void StopDepressurize()
    {
        //convinced nozomu
        puzzleStarted = false;
        GetComponent<AudioSource>().Stop();
        InGame_Interface.instance.StopOxygenTimer();
        AudioListener.volume = 1;
        if (windowMovingCover.AtStartPosition) windowMovingCover.Activate();
        used_window = used_gravity = used_air = true;
        GameController.GravityOn = true;
        GameController.instance.TogglePlayerGravity(true);
        computerScreenText.text = "WAITING FOR INPUT";
        computerScreenText.color = Color.white;
    }
    public ButtonProp MainDoorControl;
    public Text mainDoorText;
    public MovingObject MainDoorMovingObject;
    void EnableDoorUnlock()
    {
        MainDoorControl.requiresPower = false;
        MainDoorControl.oneUseOnly = true;
        MainDoorControl.used = false;
        MainDoorControl.buttonWorking = true;
        MainDoorControl.activationEvent = "OpenDoor";
        mainDoorText.color = Color.white;
    }
    void OpenDoor()
    {
        MainDoorMovingObject.Activate();
        MusicManager.instance.Music_OpenDoor(0.5f);
    }
    void BatteryEvent()
    {
        PlayerData.tookBatteryToExit = true;
    }
    void OutroConversation()
    {
        AI_Window.instance.SetChatName("???");
        if (!PlayerData.consideredHostile)
        {
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "The Vault is in a.. &&pretty shaken state. &&We have a lot to do before a real threat shows up.",
                emotion = Emotion.normal,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 2
            });
            if (PlayerData.refusedToTellAnything)
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "You better be more cooperative then last time.",
                    emotion = Emotion.annoyed,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 3
                });
            }
            else if (PlayerData.wasASmartass)
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "You better stop being a jackass, though.",
                    emotion = Emotion.annoyed,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 3
                });
            }
            if (PlayerData.tookBatteryToExit && PlayerData.solvedPuzzle)
            {
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "<nickname>, I don't think you'll be needing that battery anymore.",
                    emotion = Emotion.happyOpenMouth,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 2
                });
            }
            if (!PlayerData.toldName)
            {
                //never told me your name
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "Before we continue, <nickname>, we need a way to address each other.",
                    emotion = Emotion.annoyedClosedEyes,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 3,
                    response = new SpeechReponse()
                    {
                        choices =
                    {
                        "Call me <name>."
                    },
                        choicesEventID =
                    {
                        "MyNameIs"
                    }
                    }
                });
            }
            else
            {
                //i never introduced myself
                AI_Window.instance.AddSpeechEvent(new SpeechEvent()
                {
                    message = "I just realized, <nickname>, that you have no way of addressing me.. &&Oh well.",
                    emotion = Emotion.annoyedClosedEyes,
                    letterPause = NozomuData.textSpeed,
                    timeUntilNextSpeech = 3,
                    eventOnSpeechFinished = "MyNameIs"
                });
            }
        }
        else
        {
            MainDoorMovingObject.Activate();
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "You're not gonna get far, I promise you that. &&The entire facility is on high alert.",
                emotion = Emotion.annoyed,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 3
            });
            AI_Window.instance.AddSpeechEvent(new SpeechEvent()
            {
                message = "Enjoy the last hours of your life...",
                emotion = Emotion.noCharacter,
                letterPause = NozomuData.textSpeed,
                timeUntilNextSpeech = 4,
                eventOnSpeechFinished = "EndFadeOut"
            });
        }
        AI_Window.instance.TryNextSpeechEvent();
    }
    void MyNameIs()
    {
        AI_Window.instance.SetChatName("Nozomu");
        AI_Window.instance.AddSpeechEvent(new SpeechEvent()
        {
            message = "My name is Nozomu.",
            emotion = Emotion.annoyedSmile,
            letterPause = NozomuData.textSpeed,
            timeUntilNextSpeech = 4,
            eventOnSpeechFinished = "EndFadeOut"
        });
        AI_Window.instance.TryNextSpeechEvent();
    }
    void EndFadeOut()
    {
        InGame_Interface.instance.DeathScreen("The End");
        GameController.AllowPlayerInput = false;
    }
    #endregion

    void PlayerDeath()
    {
        InGame_Interface.instance.DeathScreen();
        GetComponent<AudioSource>().Stop();
        GameController.instance.PlayerRBController.gameObject.SetActive(false);
        AudioListener.volume = 0;
    }

    #region Audio
    public void Music_Start()
    {
        MusicManager.instance.Music_Start(0.6f);
    }
    public void Music_Room()
    {
        MusicManager.instance.Music_Room(0.6f);
    }
    #endregion
}
