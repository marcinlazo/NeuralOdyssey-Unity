using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour {

	/* Power source that can be carried by player and placed in sockets */

	public PowerSocket parentSocket;
    public PlayerInteraction playerHolding;

    public void ToggleKinematics(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
    }
}
