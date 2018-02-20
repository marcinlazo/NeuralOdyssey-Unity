using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class InGame_Interface : MonoBehaviour {

    public static InGame_Interface instance;
    public static bool iis { get { return instance != null; } }

    [SerializeField] Image crosshair;
    [SerializeField] GameObject panelOxygen;
    [SerializeField] Text oxygen;
    [SerializeField] CanvasGroup deathScreen;
    [SerializeField] CanvasGroup gameOver;
    [SerializeField] Button restartButton;
    [SerializeField] GameObject ESC_Panel;
    [SerializeField] Text logText;
    [SerializeField] Text hintText;
    bool oxygenTimer;

    bool escMenuOn;
    StringBuilder sb = new StringBuilder(200);

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        ShowHint("Move: WSAD\nJump: Space\nLeft Mouse: Interact\nRight Mouse: Drop\n", 45, 6);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escMenuOn = !escMenuOn;
            ShowEscape(escMenuOn);
        }
    }
    void ShowEscape(bool show)
    {
        ESC_Panel.SetActive(show);
        Time.timeScale = show ? 0 : 1;
        StaticUI.ShowCursor(show);
    }
    public void ShowHint(string message, float howLong, float delay = 0)
    {
        StartCoroutine(ShowHintForSeconds(message, howLong, delay));
    }
    IEnumerator ShowHintForSeconds(string message, float howLong, float delay)
    {
        if (delay > 0) yield return new WaitForSeconds(delay);
        hintText.text = message;
        yield return new WaitForSeconds(howLong);
        hintText.text = "";
    }
    public void ChangeCrosshair(Color color)
    {
        if(crosshair.color != color) crosshair.color = color;
    }
    public void AddLogText(string speaker, string message)
    {
        sb.Append(string.Format("\n> {0}: {1}", speaker, message));
        logText.text = sb.ToString();
    }
    public void StartOxygenTimer(float time)
    {
        panelOxygen.SetActive(true);
        oxygenTimer = true;
        StartCoroutine(OxygenTimer(time));
    }
    IEnumerator OxygenTimer(float time)
    {
        while(time > 0 && oxygenTimer)
        {
            oxygen.text = "Oxygen: \n" + System.Math.Round(time, 0);
            time -= Time.deltaTime;
            yield return null;
        }
        if (oxygenTimer)
        {
            panelOxygen.SetActive(false);
            EventManager.instance.FireEvent("PlayerDeath");
        }
    }
    public void StopOxygenTimer()
    {
        oxygenTimer = false;
        panelOxygen.SetActive(false);
    }

    public void DeathScreen(string message = "GameOver")
    {
        StartCoroutine(ShowDeathScreen(message));
    }
    IEnumerator ShowDeathScreen(string message)
    {
        deathScreen.alpha = 0;
        gameOver.alpha = 0;
        gameOver.gameObject.GetComponent<Text>().text = message;
        deathScreen.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        float time = 0;
        float timeMax = 5f;
        while (time / timeMax <= 1)
        {
            deathScreen.alpha = Mathf.Lerp(0, 1, time / timeMax);
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        timeMax = 3;
        while (time / timeMax <= 1)
        {
            gameOver.alpha = Mathf.Lerp(0, 1, time / timeMax);
            time += Time.deltaTime;
            yield return null;
        }
        restartButton.gameObject.SetActive(true);
        StaticUI.ShowCursor(true);
    }
    public void Bn_RestartGame()
    {
        PlayerData.Reset();
        Debug.Log("Game Restart");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    public void Bn_Resume()
    {
        escMenuOn = false;
        ShowEscape(false);
    }
    public void Bn_Quit()
    {
        Debug.Log("Game quit");
        if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        instance = FindObjectOfType<InGame_Interface>();
    }
#endif
}
