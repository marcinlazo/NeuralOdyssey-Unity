using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] InputField inputName;
    [SerializeField] Button PlayButton;

    private void Awake()
    {
        PlayButton.interactable = false;
    }
    public void OnNameChanged()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            PlayButton.interactable = false;
        }
        else
        {
            PlayButton.interactable = true;
            PlayerData.SetName(inputName.text);
        }
    }
    public void Bn_Play()
    {
        EventManager.instance.FireEvent("StartIntro");
    }
    public void Bn_Quit()
    {
        if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
    }
}
