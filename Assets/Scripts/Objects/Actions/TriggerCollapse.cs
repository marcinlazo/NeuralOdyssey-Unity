using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollapse : MonoBehaviour {

	/* Will make objects approached my player unfreeze and fall */

	public int layer;
    public Vector3 force;
    Rigidbody rb;
    AudioSource aSource;
    bool used;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        aSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (other.gameObject.layer == layer)
        {
            rb.isKinematic = false;
            rb.AddForceAtPosition(force, other.transform.position, ForceMode.Impulse);
            if (aSource != null) aSource.Play();
            this.enabled = false;
            used = true;
        }
    }
}
