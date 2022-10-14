using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    private void FixedUpdate()
    {
        SendPlayerPosRot();
    }
}
