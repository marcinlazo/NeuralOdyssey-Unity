using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSocketTrigger : MonoBehaviour {

	/* Manual trigger for items that start in sockets */

	PowerSocket parentSocket;

    void Start()
    {
        parentSocket = GetComponentInParent<PowerSocket>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        parentSocket.TriggerEnter(other);
    }

}
