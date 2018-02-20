using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Canvas canvas;
    public static bool GravityOn = true;
    public Rigidbody PlayerRigidbody;
    public RigidbodyFirstPersonController PlayerRBController;
    public List<Rigidbody> physicsProps { get; private set; }
    public static bool AllowPlayerInput = true;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        canvas.gameObject.SetActive(true);
        PlayerData.SetNickname("");
        physicsProps = new List<Rigidbody>();
        AllowPlayerInput = true;
        GravityOn = true;
    }
    void Start ()
    {
        PrepareScene();
	}
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F1)) AI_Window.FastTalk = !AI_Window.FastTalk;
	}

    void PrepareScene()
    {
        StaticUI.ShowCursor(false);
        EventManager.instance.FireEvent("Music_Start");
    }
    public void AddPhysicProp(Rigidbody rigidbody)
    {
        physicsProps.Add(rigidbody);
    }
    public void TogglePlayerGravity(bool state)
    {
        PlayerRigidbody.useGravity = state;
        PlayerRBController.jetpackFlying = !state;
        foreach (Rigidbody rb in physicsProps) rb.useGravity = state;
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        instance = FindObjectOfType<GameController>();
    }
#endif
}
