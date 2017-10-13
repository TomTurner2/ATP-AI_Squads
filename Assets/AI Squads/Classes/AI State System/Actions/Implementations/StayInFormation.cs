using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/StayInFormation")]
public class StayInFormation : Action
{
    public override void Execute(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;
        controller.knowledge.waypoint = controller.knowledge.follow_target.position;
    }
}
