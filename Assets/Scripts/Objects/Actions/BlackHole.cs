using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

	/* Pulls in all gravity affected, targeted objects */

	[SerializeField] Rigidbody[] targets;
    Rigidbody[] targetsRB;
    bool[] targetsFlag;
    public bool blackholeOn, disableGravityOnStart;

    public float forceStart, forcePerSecond, timeUntilTurnOff;
    float time;

    private void Start()
    {
        targetsFlag = new bool[targets.Length];
        targetsRB = new Rigidbody[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            targetsRB[i] = targets[i];
            targetsFlag[i] = false;
        }
    }
    public void NewTargets(Rigidbody[] rigidbodies)
    {
        targetsFlag = new bool[rigidbodies.Length];
        targetsRB = new Rigidbody[rigidbodies.Length];
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            targetsRB[i] = rigidbodies[i];
            targetsFlag[i] = false;
        }
    }
    private void Update()
    {
        if (blackholeOn)
        {
            for (int i = 0; i < targetsFlag.Length; i++)
            {
                if (!targetsFlag[i])
                {
                    if (disableGravityOnStart) targetsRB[i].useGravity = false;
                    Vector3 force = transform.position - targetsRB[i].position;
                    force.Normalize();
                    force *= forceStart;
                    if (targetsRB[i].gameObject.CompareTag("Player")) force *= 1f;
                    targetsRB[i].AddForce(force, ForceMode.Force);
                    if(Vector3.Distance(transform.position, targetsRB[i].position) < 1)
                    {
                        targetsRB[i].useGravity = false;
                        targetsFlag[i] = true;
                    }
                }
            }
            time += Time.deltaTime;
            forceStart += forcePerSecond * Time.deltaTime;

            if(time > timeUntilTurnOff)
            {
                for (int i = 0; i < targetsFlag.Length; i++)
                {
                    targetsRB[i].useGravity = false;
                    targetsFlag[i] = true;
                }
                blackholeOn = false;
            }
        }
    }
}
