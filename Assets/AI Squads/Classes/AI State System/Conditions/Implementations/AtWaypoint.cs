using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AtWaypoint")]
public class AtWaypoint : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        return (Vector3.Distance(controller.transform.position, controller.knowledge.waypoint) <=
                controller.knowledge.target_arrival_tolerance);
    }
}
