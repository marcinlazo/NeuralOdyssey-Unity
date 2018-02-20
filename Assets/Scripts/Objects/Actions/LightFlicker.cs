using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
    new Light light;
    bool changing;
    public float minIntensity = 0.5f, maxIntensity = 1.5f;
    public float minTime = 0.5f, maxTime = 1.5f;

    private void Awake()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && !changing) StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        changing = true;
        float time = 0;
        float timeMax = Random.Range(minTime, maxTime);
        float startIntensity = light.intensity;
        float targetIntensity = Random.Range(minIntensity, maxIntensity);
        while (time / timeMax <= 1)
        {
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / timeMax);
            time += Time.deltaTime;
            yield return null;
        }
        changing = false;
    }
}
