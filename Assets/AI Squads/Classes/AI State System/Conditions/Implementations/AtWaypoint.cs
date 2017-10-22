using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AtWaypoint")]
public class AtWaypoint : Condition
{
    public override bool Check(Knowledge _controller)
    {
        return (Vector3.Distance(_controller.ai_controller.transform.position, _controller.waypoint) <=
                _controller.target_arrival_tolerance);
    }
}
