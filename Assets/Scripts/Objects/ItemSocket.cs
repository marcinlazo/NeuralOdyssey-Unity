﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSocket : MonoBehaviour
{
    public Transform socket;
    public Text text;
    ItemSource itemSource;
    public bool lockItem = false;
    public string[] takesItems;

    public GameObject itemDestination;

    private void Awake()
    {
        if (socket == null) socket = transform;
        Text("OFFLINE", Color.red);
    }

    public bool HasPower { get { return itemSource != null; } }
    public bool GiveItemSource(ItemSource source)
    {
        if (!HasPower)
        {
            source.transform.position = socket.position;
            source.transform.SetParent(socket);
            source.transform.rotation = Quaternion.identity;
            itemSource = source;
            itemSource.parentSocket = this;
            if (itemSource.playerHolding != null) itemSource.playerHolding.TakeProp();
            itemSource.ToggleKinematics(true);
            //if (!lockItem) Text("ONLINE", Color.green);
            //else Text("LOCKED", Color.green);
            //if (poweredObject != null) poweredObject.GivePower(this);
            return true;
        }
        else return false;
    }
    public ItemSource TakePowerSource()
    {
        if (lockItem) return null;
        itemSource.transform.SetParent(null);
        itemSource.parentSocket = null;
        itemSource.ToggleKinematics(false);
        ItemSource outSource = itemSource;
        itemSource = null;
        //Text("OFFLINE", Color.red);
        //if (poweredObject != null) poweredObject.TakePower(this);
        return outSource;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!HasPower)
        {
            ItemSource newSource = other.gameObject.GetComponent<ItemSource>();
            if (newSource != null)
            {
                GiveItemSource(newSource);
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
}