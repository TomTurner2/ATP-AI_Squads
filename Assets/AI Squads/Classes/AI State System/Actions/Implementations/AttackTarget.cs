using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/Actions/AttackTarget")]
    public class AttackTarget : Action
    {
        public override void Execute(Knowledge _controller)
        {
            if (_controller == null)
                return;

            if (_controller.ai_controller.controlled_character.dead)
            {
                _controller.is_shooting = false;
                return;
            }

            //TODO override auto crouch

            FireWeapon(ref _controller);
        }


        private List<Collider> GetLineOfSightIgnoreColliders(ref Knowledge _controller)
        {
            List<Collider> ignore = new List<Collider>();

            Collider character_collider = _controller.ai_controller.controlled_character.character_collider;
            if (character_collider != null)
                ignore.Add(character_collider);


            Collider enemy_collider = _controller.closest_enemy.character_collider;
            if (enemy_collider != null)
                ignore.Add(enemy_collider);

            return ignore;
        }


        private void FireWeapon(ref Knowledge _controller)
        {
            if (!_controller.burst_fire_cooldown_timer.UpdateTimer())
            {
                _controller.is_shooting = false;
                return;
            }

            _controller.is_shooting = true;

            Vector3 enemy_pos = _controller.closest_enemy.transform.position;
            Vector3 aim_target = new Vector3(enemy_pos.x, enemy_pos.y + 1f, enemy_pos.z);
            Vector3 look_target = new Vector3(enemy_pos.x, _controller.ai_controller.transform.position.y, enemy_pos.z);

            _controller.ai_controller.transform.LookAt(look_target);
            _controller.ai_controller.weapon.Aim(aim_target);
            _controller.ai_controller.weapon.Shoot();

            ++_controller.shot_count;

            if (_controller.shot_count < _controller.max_shot_count)
                return;

            _controller.shot_count = 0;
            _controller.burst_fire_cooldown_timer.Reset();
        }
    }
}
