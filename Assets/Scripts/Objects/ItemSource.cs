using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSource : MonoBehaviour {

    public string itemID;
    public ItemSocket parentSocket;
    public PlayerInteraction playerHolding;

    public void ToggleKinematics(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
    }
}
