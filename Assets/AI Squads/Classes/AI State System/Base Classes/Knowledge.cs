using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/Knowledge/SquadMemberKnowledge")]
    public class Knowledge : ScriptableObject
    {   
        [Tooltip("Tolerance for arriving at target")]
        public float target_arrival_tolerance = 0.5f;
        [Tooltip("Delay between gun fire bursts")]
        public float burst_fire_cooldown = 2f;
        [Tooltip("Maximum shots taken in a burst")]
        public int max_shot_count = 5;
        [Tooltip("Max distance to check for an enemy")]
        public float enemy_detect_radius = 100f;

        [HideInInspector] public int shot_count = 0;
        [HideInInspector] public bool can_take_cover = true;
        [HideInInspector] public bool can_fire = true;
        [HideInInspector] public bool is_shooting = false;
        [HideInInspector] public Vector3 waypoint { get; set; }
        [HideInInspector] public Vector3 best_cover { get; set; }
        [HideInInspector] public Transform follow_target { get; set; }
        [HideInInspector] public CountdownTimer burst_fire_cooldown_timer = new CountdownTimer();
        [HideInInspector] public Character closest_enemy = null;
    }
}
