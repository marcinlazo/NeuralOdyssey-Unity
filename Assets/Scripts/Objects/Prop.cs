using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Prop : MonoBehaviour, IThrowable<Prop>
{
    public Rigidbody propRigidbody { get; private set; }
    Collider col;
    [SerializeField] public bool HeavyObject = false;
    float startDrag;
    float startAngularDrag;

    void Start()
    {
        propRigidbody = GetComponent<Rigidbody>();
        startDrag = propRigidbody.drag;
        startAngularDrag = propRigidbody.angularDrag;
        col = GetComponent<Collider>();
        GameController.instance.AddPhysicProp(propRigidbody);
    }

    public void Drag(Vector3 velocity)
    {
        throw new System.NotImplementedException();
    }

    public Prop PickUp()
    {
        if (HeavyObject) return null;
        ToggleKinematic(true);
        gameObject.layer = 9; //PropHeld
        col.enabled = true;
        propRigidbody.angularDrag = 5;
        return this;
    }

    public void Push(Vector3 force)
    {
        gameObject.layer = 8; //Prop
        ToggleKinematic(false);
        col.enabled = true;
        propRigidbody.angularDrag = startAngularDrag;
        propRigidbody.AddForce(force, ForceMode.VelocityChange);
    }

    public void Drop()
    {
        gameObject.layer = 8; //Prop
        col.enabled = true;
        propRigidbody.angularDrag = startAngularDrag;
        ToggleKinematic(false);
    }

    void ToggleKinematic(bool state)
    {
        propRigidbody.useGravity = !state? GameController.GravityOn : false;
        //propRigidbody.isKinematic = state;
    }
}
