using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozomuData : MonoBehaviour {

    public static NozomuData instance;

    [SerializeField] EmotionPack[] emotionPack;
    static Dictionary<Emotion, EmotionPack> emotionDictionary;

    public static float textSpeed = 0.05f;

    private void Awake()
    {
        instance = this;
        PrepareData();
    }
    void PrepareData()
    {
        emotionDictionary = new Dictionary<Emotion, EmotionPack>();
        foreach (EmotionPack em in emotionPack)
        {
            emotionDictionary.Add(em.emotion, em);
        }
    }
    public static Texture GetEmotion(Emotion emotion)
    {
        EmotionPack pack = emotionDictionary[emotion];
        return pack.textures[Random.Range(0, pack.textures.Length)];
    }
    public static Texture GetEmotionFinal(Emotion emotion)
    {
        EmotionPack pack = emotionDictionary[emotion];
        return pack.textures[pack.finalEmotion];
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        instance = FindObjectOfType<NozomuData>();
    }
#endif
}