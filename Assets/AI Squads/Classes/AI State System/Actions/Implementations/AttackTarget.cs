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

            if (controller.knowledge.closest_enemy == null)
                controller.knowledge.closest_enemy = GameManager.scene_refs.FactionManager.FindClosestEnemy(controller.transform.position,
                    controller.controlled_character.faction, controller.knowledge.enemy_detect_radius);

            if (controller.knowledge.closest_enemy == null)
                return;

            if (controller.weapon == null)
                return;

            //TODO find optimal engagement position and set waypoint?
            //TODO check line of sight on target

            FireWeapon(ref controller);
        }


        private void FireWeapon(ref AIController _controller)
        {
            if (!_controller.knowledge.burst_fire_cooldown_timer.UpdateTimer())
            {
                _controller.knowledge.is_shooting = false;
                return;
            }

            _controller.knowledge.is_shooting = true;
            _controller.transform.LookAt(_controller.knowledge.closest_enemy.transform.position);
            _controller.weapon.Aim(_controller.knowledge.closest_enemy.transform.position);
            _controller.weapon.Shoot();

            ++_controller.knowledge.shot_count;

            if (_controller.knowledge.shot_count > _controller.knowledge.max_shot_count)
                _controller.knowledge.burst_fire_cooldown_timer.Reset();
        }
    }
}
