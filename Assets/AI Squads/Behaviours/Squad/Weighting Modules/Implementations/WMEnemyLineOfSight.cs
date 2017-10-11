using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weight Module/WMEnemyLineOfSight")]
public class WMEnemyLineOfSight : WeightModule
{
    [SerializeField] float height_offset = 0.1f;
    [SerializeField] float enemy_max_check_distance = 100;

    [HideInInspector]public Vector3 debug_start = Vector3.zero;
    [HideInInspector]public Vector3 debug_end = Vector3.zero;


    public override int AssessWeight(TacticalAssessor.WeightedPoint _point, float _area_radius,
        ref List<TacticalAssessor.WeightedPoint> _weighted_positions, Faction _requesters_faction)
    {
        if (_requesters_faction == null)
            return 0;

        return VisibleToEnemy(_point.position, _requesters_faction) ? weight_penalty : 0;
    }


    private bool VisibleToEnemy(Vector3 _position, Faction _requesters_faction)
    {
        GameObject closest_enemy = GameManager.scene_refs.FactionManager.FindClosestEnemy(_position,
            _requesters_faction, enemy_max_check_distance);

        if (closest_enemy == null)
            return false;

        float enemy_height = 2;
        CapsuleCollider enemy_capsule_collider = closest_enemy.GetComponentInChildren<CapsuleCollider>();

        if (enemy_capsule_collider != null)
            enemy_height = enemy_capsule_collider.height;

        Vector3 ray_origin = new Vector3(closest_enemy.transform.position.x,
            closest_enemy.transform.position.y + enemy_height, closest_enemy.transform.position.z);

        Vector3 target = new Vector3(_position.x, _position.y + height_offset, _position.z);//offset target from ground
        Vector3 direction = target - ray_origin;
        float dist = Vector2.Distance(target, ray_origin);

        debug_start = ray_origin;
        debug_end = ray_origin + (direction.normalized * dist);

        bool result = Physics.Raycast(ray_origin, direction.normalized, dist);
        return result;
    }

}
