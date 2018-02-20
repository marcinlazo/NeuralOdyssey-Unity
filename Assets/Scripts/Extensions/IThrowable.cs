using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable<T> {

    void Drag(Vector3 velocity);
    void Push(Vector3 force);
    T PickUp();
    void Drop();
}
