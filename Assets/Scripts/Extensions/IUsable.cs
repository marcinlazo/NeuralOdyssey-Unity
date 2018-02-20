using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable {

    void Activate();
    bool CanBeActivated();
    bool NeedsPower();
}
