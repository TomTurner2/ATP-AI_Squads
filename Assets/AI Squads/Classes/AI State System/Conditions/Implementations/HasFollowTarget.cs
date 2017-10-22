using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/HasFollowTarget")]
public class HasFollowTarget : Condition
{
    public override bool Check(Knowledge _controller)
    {
        return _controller.follow_target != null;
    }
}
