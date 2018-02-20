using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManagementPuzzle : MonoBehaviour, IPowered
{
    public GameObject[] toActivate;
    public string eventOnPower;
    public string eventOffPower;

    public void GivePower(PowerSocket socket)
    {
        StartPuzzle();
        EventManager.instance.FireEvent(eventOnPower);
    }

    public void TakePower(PowerSocket socket)
    {
        EventManager.instance.FireEvent(eventOffPower);
        throw new System.NotImplementedException();
    }

    void StartPuzzle()
    {
        foreach(GameObject o in toActivate)
        {
            o.SetActive(true);
        }
    }
}
