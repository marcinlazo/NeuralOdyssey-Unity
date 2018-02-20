using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSocketTrigger : MonoBehaviour {

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
