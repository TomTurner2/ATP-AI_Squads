using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/DistanceToWaypoint")]
public class DistanceToWaypoint : Condition
{
    [SerializeField] float distance = 10; 
    public override bool Check(Knowledge _controller)
    {
        return Vector3.Distance(_controller.ai_controller.transform.position, _controller.waypoint) <=
               distance;
    }
}
