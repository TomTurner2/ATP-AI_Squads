using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;


[CreateAssetMenu(menuName = "AIStateSystem/Conditions/LineOfSightOnEnemy")]
public class LineOfSightOnEnemy : Condition
{
    [SerializeField] private float eye_height = 1.5f;


    public override bool Check(Knowledge _controller)
    {

        if (_controller.closest_enemy == null)
            return false;

        return LineOfSightCheck.CheckLineOfSight(_controller.ai_controller.transform.position + new Vector3(0, eye_height, 0),
            _controller.closest_enemy.transform.position + new Vector3(0, eye_height, 0),
            GetLineOfSightIgnoreColliders(ref _controller));
    }


    private List<Collider> GetLineOfSightIgnoreColliders(ref Knowledge _controller)
    {
        List<Collider> ignore = new List<Collider>();

        Collider character_collider = _controller.ai_controller.controlled_character.character_collider;
        if (character_collider != null)
            ignore.Add(character_collider);


        Collider enemy_collider = _controller.ai_controller.knowledge.closest_enemy.character_collider;
        if (enemy_collider != null)
            ignore.Add(enemy_collider);

        return ignore;
    }
}
