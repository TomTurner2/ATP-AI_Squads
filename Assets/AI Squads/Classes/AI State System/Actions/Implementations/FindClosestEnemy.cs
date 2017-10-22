using System.Collections;
using System.Collections.Generic;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/FindClosestEnemy")]
public class FindClosestEnemy : Action
{
    public override void Execute(Knowledge _controller)
    {
        if (_controller == null)
            return;

        _controller.closest_enemy = GameManager.scene_refs.FactionManager.FindClosestEnemy(
            _controller.ai_controller.transform.position,_controller.ai_controller.controlled_character.faction,
            _controller.enemy_detect_radius);
    }
}
