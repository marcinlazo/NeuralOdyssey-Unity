using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProp : MonoBehaviour, IUsable {

    public GameObject someObject;
    IUsable interactableObject = null;
    AudioSource audioSource;
    public bool requiresPower, oneUseOnly;
    public string activationEvent;
    public bool used;
    public PowerSocket powerSocket;
    public AudioClip activate, cantActivate;
    public bool buttonWorking = true;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(someObject != null) interactableObject = someObject.GetComponent<IUsable>();
    }

    public void Activate()
    {
        if (buttonWorking && (interactableObject == null || interactableObject.CanBeActivated()) && (!requiresPower || (requiresPower && HasPower())) && (!oneUseOnly || (oneUseOnly && !used)))
        {
            if(interactableObject != null) interactableObject.Activate();
            PlaySound(activate);
            EventManager.instance.FireEvent(activationEvent);
            used = true;
        }
        else
        {
            PlaySound(cantActivate);
        }
    }
    public bool CanBeActivated()
    {
        if (interactableObject == null) return true;
        else return interactableObject.CanBeActivated();
    }

    public bool NeedsPower()
    {
        return requiresPower;
    }
    public bool HasPower()
    {
        if (!requiresPower) return true;
        else return powerSocket.HasPower;
    }
    void PlaySound(AudioClip clip)
    {
        if (audioSource)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
