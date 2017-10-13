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
            AIController controller =  _controller as AIController;

            if (controller == null)
                return;

            controller.nav_mesh_agent.isStopped = false;

            if (controller.controlled_character.dead)
            {
                controller.nav_mesh_agent.isStopped = true;
                return;
            }

            if (Vector3.Distance(controller.transform.position, controller.knowledge.waypoint) >=
                controller.knowledge.target_arrival_tolerance)
                controller.MoveToPosition(controller.knowledge.waypoint);
        }
    }
}
