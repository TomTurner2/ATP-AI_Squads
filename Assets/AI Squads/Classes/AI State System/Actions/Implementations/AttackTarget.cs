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
            AIController controller = _controller as AIController;

            if (controller == null)
                return;

            if (controller.controlled_character.dead)
            {
                controller.knowledge.is_shooting = false;
                return;
            }

            if (controller.knowledge.closest_enemy == null || controller.knowledge.closest_enemy.dead)
                return;

            if (controller.weapon == null)
                return;

            //if (!LineOfSightCheck.CheckLineOfSight(controller.transform.position + new Vector3(0, 1, 0),
            //    controller.knowledge.closest_enemy.transform.position, GetLineOfSightIgnoreColliders(ref controller)))
            //{
            //    //TODO override auto crouch
            //    //TODO find optimal engagement position and set waypoint
            //    return;
            //}

            FireWeapon(ref controller);
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


        private void FireWeapon(ref AIController _controller)
        {
            if (!_controller.knowledge.burst_fire_cooldown_timer.UpdateTimer())
            {
                _controller.knowledge.is_shooting = false;
                return;
            }

            _controller.knowledge.is_shooting = true;

            Vector3 enemy_pos = _controller.knowledge.closest_enemy.transform.position;
            Vector3 aim_target = new Vector3(enemy_pos.x, enemy_pos.y + 1f, enemy_pos.z);
            Vector3 look_target = new Vector3(enemy_pos.x, _controller.transform.position.y, enemy_pos.z);

            _controller.transform.LookAt(look_target);
            _controller.weapon.Aim(aim_target);
            _controller.weapon.Shoot();

            ++_controller.knowledge.shot_count;

            if (_controller.knowledge.shot_count < _controller.knowledge.max_shot_count)
                return;

            _controller.knowledge.shot_count = 0;
            _controller.knowledge.burst_fire_cooldown_timer.Reset();
        }
    }
}
