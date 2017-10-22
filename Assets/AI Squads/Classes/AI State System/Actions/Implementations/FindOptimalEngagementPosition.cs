using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AIStateSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "AIStateSystem/Actions/FindOptimalEngagementPosition")]
public class FindOptimalEngagementPosition : Action
{
    public override void Execute(Knowledge _controller)
    {

        if (_controller.ai_controller.controlled_character.dead)
            return;

        _controller.waypoint = GameManager.scene_refs.tactical_assessor.
            FindOptimalCoverInArea(_controller.ai_controller.controlled_character.transform.position,
            1, _controller.ai_controller.controlled_character.faction).First();
    }
}
