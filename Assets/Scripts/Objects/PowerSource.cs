using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour {

    public PowerSocket parentSocket;
    public PlayerInteraction playerHolding;

    public void ToggleKinematics(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
    }
}
