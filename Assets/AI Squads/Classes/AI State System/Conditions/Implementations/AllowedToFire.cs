using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AllowedToFire")]
public class AllowedToFire : Condition
{

    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        if (controller == null)
            return false;

        return controller.knowledge.can_fire;
    }
}
