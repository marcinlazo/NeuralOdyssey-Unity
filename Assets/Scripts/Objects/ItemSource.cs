using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSource : MonoBehaviour {

	/* Framework for a general type ItemSource (similar to PowerSource) which was never expanded */

	public string itemID;
    public ItemSocket parentSocket;
    public PlayerInteraction playerHolding;

    public void ToggleKinematics(bool state)
    {
        GetComponent<Rigidbody>().isKinematic = state;
    }
}
