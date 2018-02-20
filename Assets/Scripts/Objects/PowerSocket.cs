using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerSocket : MonoBehaviour {

    public Transform socket;
    public Text text;
    public string eventPowerOn, eventPowerOff;
    PowerSource powerSource;
    AudioSource audioSource;
    public AudioClip getPower, losePower;
    public bool lockPower = false;

    public GameObject powerDestination;
    IPowered poweredObject;

    private void Start()
    {
        if (socket == null) socket = transform;
        audioSource = GetComponent<AudioSource>();
        PowerSource power = socket.GetComponentInChildren<PowerSource>();
        Text("OFFLINE", Color.red);
        if (power != null) EnablePowerSource(power, false);
        if (powerDestination) poweredObject = powerDestination.GetComponent<IPowered>();
    }

    public bool HasPower { get { return powerSource != null; } }
    public bool EnablePowerSource(PowerSource source, bool playSound = true)
    {
        if (!HasPower)
        {
            source.transform.position = socket.position;
            source.transform.SetParent(socket);
            source.transform.rotation = Quaternion.identity;
            powerSource = source;
            powerSource.parentSocket = this;
            if (powerSource.playerHolding != null) powerSource.playerHolding.TakeProp();
            powerSource.ToggleKinematics(true);
            powerSource.gameObject.layer = 8; //Prop
            if (!lockPower) Text("ONLINE", Color.green);
            else Text("LOCKED", Color.green);
            if (poweredObject != null) poweredObject.GivePower(this);
            if (playSound) PlaySound(getPower);
            EventManager.instance.FireEvent(eventPowerOn);
            return true;
        }
        else return false;
    }
    public PowerSource DisablePowerSource()
    {
        if (lockPower) return null;
        powerSource.transform.SetParent(null);
        powerSource.parentSocket = null;
        powerSource.ToggleKinematics(false);
        PowerSource outSource = powerSource;
        powerSource = null;
        Text("OFFLINE", Color.red);
        if (poweredObject != null) poweredObject.TakePower(this);
        PlaySound(losePower);
        EventManager.instance.FireEvent(eventPowerOff);
        return outSource;
    }
    public void LockPowerCommand(bool state)
    {
        lockPower = state;
        if(powerSource == null) Text("OFFLINE", Color.red);
        else Text("ONLINE", Color.green);
    }
    public void TriggerEnter(Collider other)
    {
        if (!HasPower)
        {
            PowerSource newSource = other.gameObject.GetComponent<PowerSource>();
            if (newSource != null)
            {
                EnablePowerSource(newSource);
            }
        }
    }
    void Text(string message, Color color)
    {
        if (text != null)
        {
            text.text = message;
            text.color = color;
        }
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
