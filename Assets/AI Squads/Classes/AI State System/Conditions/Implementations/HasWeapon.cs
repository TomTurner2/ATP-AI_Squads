using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/HasWeapon")]
public class HasWeapon : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        if (controller == null)
            return false;

        return controller.weapon != null;//return true if the controller has a weapon
    }
}
