using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/LineOfSightOnEnemy")]
public class LineOfSightOnEnemy : Condition
{
    public override bool Check(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        if (controller.knowledge.closest_enemy == null)
            return false;

        return LineOfSightCheck.CheckLineOfSight(controller.transform.position + new Vector3(0, 1, 0),
            controller.knowledge.closest_enemy.transform.position, GetLineOfSightIgnoreColliders(ref controller));
    }


    private List<Collider> GetLineOfSightIgnoreColliders(ref AIController _controller)
    {
        List<Collider> ignore = new List<Collider>();

        Collider character_collider = _controller.controlled_character.character_collider;
        if (character_collider != null)
            ignore.Add(character_collider);


        Collider enemy_collider = _controller.knowledge.closest_enemy.character_collider;
        if (enemy_collider != null)
            ignore.Add(enemy_collider);

        return ignore;
    }
}
