﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AIStateSystem;
using UnityEngine;

namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/Actions/MoveToPosition")]
    public class MoveToPosition : Action
    {
        public override void Execute(MonoBehaviour _controller)
        {
            AIController controller = (AIController) _controller;

            if (controller == null)
                return;

            if (controller.is_shooting)
                return;

            controller.nav_mesh_agent.isStopped = false;

            if (controller.controlled_character.dead)
            {
                controller.nav_mesh_agent.isStopped = true;
                return;
            }

            if (Vector3.Distance(controller.transform.position, controller.waypoint) >=
                controller.target_arrival_threshold)
                controller.MoveToPosition(controller.waypoint);
        }
    }
}
