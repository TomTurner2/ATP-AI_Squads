using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/FindClosestEnemy")]
public class FindClosestEnemy : Action
{
    public override void Execute(MonoBehaviour _controller)
    {
        AIController controller = _controller as AIController;

        if (controller == null)
            return;

        controller.knowledge.closest_enemy = GameManager.scene_refs.FactionManager.FindClosestEnemy(controller.transform.position,
            controller.controlled_character.faction, controller.knowledge.enemy_detect_radius);
    }
}
