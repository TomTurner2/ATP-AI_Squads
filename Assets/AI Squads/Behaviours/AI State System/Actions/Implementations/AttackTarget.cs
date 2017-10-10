using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/Actions/AttackTarget")]
    public class AttackTarget : Action
    {
        public override void Execute(MonoBehaviour _controller)
        {
            AIController controller = (AIController)_controller;

            if (controller == null)
                return;

            if (controller.closest_enemy == null)
                controller.closest_enemy = GameManager.scene_refs.enemy_manager.FindClosestEnemy(controller.transform.position);

            if (controller.closest_enemy == null)
                return;

            if (!controller.burst_fire_cooldown_timer.UpdateTimer())
            {
                controller.is_shooting = false;
                return;
            }

            //find optimal engagement position and set waypoint?

            //if line of sight

            controller.is_shooting = true;
            controller.transform.LookAt(controller.closest_enemy.transform.position);
            //controller.gun_muzzle look at target/aim gun at target
            //controller.fire gun
            ++controller.shot_count;

            if (controller.shot_count > controller.max_shot_count)
                controller.burst_fire_cooldown_timer.Reset();
        }
    }
}
