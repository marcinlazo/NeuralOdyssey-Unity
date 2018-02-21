using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour, IUsable {

	/* Object that travels along set path when activated */

	Vector3 startPos;
    Quaternion startRot;
    public Vector3 endPos;
    public Vector3 endRotation;
    Quaternion endRot;
    public float speed;
    public AnimationCurve curve;
    public float curveTime;
    AudioSource audioSource;
    bool atStart = true;
    public bool AtStartPosition { get { return atStart; } }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        endRot = Quaternion.Euler(endRotation);
    }

    public bool IsMoving { get; private set; }

    public void Activate()
    {
        StartCoroutine(Moving());
    }
    public bool CanBeActivated() { return !IsMoving; }
    IEnumerator Moving()
    {
        IsMoving = true;
        Vector3 targetPos = atStart ? endPos : startPos;
        Vector3 starting = transform.localPosition;
        Quaternion startingRot = transform.localRotation;
        Quaternion targetRot = atStart ? endRot : startRot;
        atStart = !atStart;
        if (audioSource != null) audioSource.Play();
        float time = 0;
        while(time/curveTime <= 1)
        {
            transform.localPosition = Vector3.Lerp(starting, targetPos, time / curveTime * curve.Evaluate(time/curveTime));
            transform.localRotation = Quaternion.Lerp(startingRot, targetRot, time / curveTime * curve.Evaluate(time / curveTime));
            time += Time.deltaTime;
            yield return null;
        }
        
        IsMoving = false;
    }

    public bool NeedsPower()
    {
        return false;
    }
}
