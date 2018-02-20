using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowered {

    void GivePower(PowerSocket socket);
    void TakePower(PowerSocket socket);
}
