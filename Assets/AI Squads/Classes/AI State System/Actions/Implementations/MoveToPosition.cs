using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AIStateSystem;
using UnityEngine;

namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/Actions/MoveToPosition")]
    public class MoveToPosition : Action
    {
        public override void Execute(Knowledge _controller)
        {
            if (_controller == null)
                return;

            if (!_controller.ai_controller.nav_mesh_agent.enabled)
                return;

            if (!_controller.ai_controller.nav_mesh_agent.enabled)
                return;

            _controller.ai_controller.nav_mesh_agent.isStopped = false;

            if (_controller.ai_controller.controlled_character.dead)
            {
                _controller.ai_controller.nav_mesh_agent.isStopped = true;
                return;
            }

            if (Vector3.Distance(_controller.ai_controller.transform.position, _controller.ai_controller.knowledge.waypoint) >=
                _controller.target_arrival_tolerance)
                _controller.ai_controller.MoveToPosition(_controller.waypoint);
        }
    }
}
