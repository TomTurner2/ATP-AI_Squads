using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/ViableEnemyTarget")]
public class ViableEnemyTarget : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        if (controller == null)
            return false;

        return !(controller.knowledge.closest_enemy == null ||
            controller.knowledge.closest_enemy.dead);//return false if not a viable target
    }
}
