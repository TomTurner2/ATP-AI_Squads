using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/StayInFormation")]
public class StayInFormation : Action
{
    public override void Execute(Knowledge _controller)
    {
        _controller.waypoint = _controller.follow_target.position;
    }
}
