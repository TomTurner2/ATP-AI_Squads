using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Conditions/AllowedToTakeCover")]
public class AllowedToTakeCover : Condition
{
    public override bool Check(Knowledge _controller)
    {
        return _controller.can_take_cover;
    }
}
