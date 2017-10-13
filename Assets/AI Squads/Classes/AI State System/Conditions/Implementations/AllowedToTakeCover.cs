using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AllowedToTakeCover")]
public class AllowedToTakeCover : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller  = _controller as AIController;
        return controller.knowledge.stick_to_cover;
    }
}
