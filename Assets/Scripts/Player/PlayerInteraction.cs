using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    [SerializeField] float raycastDistance = 2f;
    [SerializeField] float throwForce = 5f;
    [SerializeField] float carrySpeed = 500f;
    [SerializeField] Transform holdingSpot;
    Camera cam;
    Prop heldProp;
    Quaternion pickUpRot;
    bool HoldingProp { get { return heldProp != null; } }

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }
    void Update()
    {
        PlayerInput();
    }
    private void FixedUpdate()
    {
        if (HoldingProp) MoveProp();
    }
    void PlayerInput()
    {
        bool fire = Input.GetKeyDown(KeyCode.Mouse0);
        bool fireAlt = Input.GetKeyDown(KeyCode.Mouse1);

        if (!HoldingProp) RaycastForProp(fire);
        else if (fire && HoldingProp) ThrowProp();
        else if (fireAlt && HoldingProp) DropProp();
    }

    private void MoveProp()
    {
        Vector3 dir = holdingSpot.position - heldProp.propRigidbody.position;
        heldProp.propRigidbody.velocity = dir * Time.deltaTime * carrySpeed;
        //heldProp.transform.position = holdingSpot.transform.position;
        //heldProp.transform.rotation = holdingSpot.transform.rotation;
    }
    private void RaycastForProp(bool fire)
    {
        LayerMask hitMask = LayerMask.GetMask("Prop", "Default");
        RaycastHit hit;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 0.1f);
        if (Physics.Raycast(ray, out hit, raycastDistance, hitMask))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f);
            Prop prop = hit.collider.GetComponentInParent<Prop>();
            ButtonProp button = hit.collider.GetComponentInParent<ButtonProp>();
            PowerSource source = hit.collider.GetComponentInParent<PowerSource>();
            if (prop != null)
            {
                if (!prop.HeavyObject)
                    InGame_Interface.instance.ChangeCrosshair(Color.cyan);

                if (fire)
                {
                    if (prop.HeavyObject)
                    {
                        prop.Push((prop.transform.position - transform.position) * throwForce);
                    }
                    else
                    {
                        
                        if (source != null)
                        {
                            if(source.parentSocket == null || (source.parentSocket != null && !source.parentSocket.lockPower))
                            {
                                if(source.parentSocket != null) source.parentSocket.DisablePowerSource();
                                source.playerHolding = this;
                                heldProp = prop.PickUp();
                                pickUpRot = heldProp.transform.rotation;
                                InGame_Interface.instance.ChangeCrosshair(Color.cyan);
                            }
                        }
                        else
                        {
                            heldProp = prop.PickUp();
                            pickUpRot = heldProp.transform.rotation;
                            InGame_Interface.instance.ChangeCrosshair(Color.cyan);
                        }                        
                    }
                }
            }
            else if(button != null)
            {
                if(button.CanBeActivated()) InGame_Interface.instance.ChangeCrosshair(Color.green);
                else InGame_Interface.instance.ChangeCrosshair(Color.yellow);

                if (fire && button.CanBeActivated()) button.Activate();
            }else InGame_Interface.instance.ChangeCrosshair(Color.gray);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.yellow, 0.1f);
            InGame_Interface.instance.ChangeCrosshair(Color.gray);
        }
    }
    private void ThrowProp()
    {
        heldProp.Push((heldProp.transform.position - transform.position)*throwForce);
        heldProp = null;
    }

    private void DropProp()
    {
        heldProp.Drop();
        heldProp = null;
    }

    public void TakeProp()
    {
        heldProp = null;
    }

    private void OnDrawGizmos()
    {
        if (holdingSpot != null) Gizmos.DrawSphere(holdingSpot.transform.position, 0.05f);
    }
}
