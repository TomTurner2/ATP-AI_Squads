using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/ViableEnemyTarget")]
public class ViableEnemyTarget : Condition
{
    public override bool Check(Knowledge _controller)
    {
        if (_controller == null)
            return false;

        return !(_controller.closest_enemy == null ||
            _controller.closest_enemy.dead);//return false if not a viable target
    }
}
