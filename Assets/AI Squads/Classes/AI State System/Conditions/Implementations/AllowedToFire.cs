using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AllowedToFire")]
public class AllowedToFire : Condition
{
    public override bool Check(Knowledge _controller)
    {
        if (_controller == null)
            return false;

        return _controller.can_fire;
    }
}
