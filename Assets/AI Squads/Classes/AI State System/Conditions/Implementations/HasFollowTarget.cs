using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/HasFollowTarget")]
public class HasFollowTarget : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;
        return controller.knowledge.follow_target != null;
    }
}
