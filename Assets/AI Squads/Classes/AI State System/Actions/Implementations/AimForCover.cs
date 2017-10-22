using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/AimForCover")]
public class AimForCover : Action
{
    public override void Execute(Knowledge _controller)
    {
        _controller.waypoint = _controller.best_cover;
    }
}
