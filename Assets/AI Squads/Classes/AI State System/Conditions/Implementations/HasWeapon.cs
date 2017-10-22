using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/HasWeapon")]
public class HasWeapon : Condition
{
    public override bool Check(Knowledge _controller)
    {
        if (_controller == null)
            return false;

        return _controller.ai_controller.weapon != null;//return true if the controller has a weapon
    }
}
