using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weight Module/WMEnemyLineOfSite")]
public class WMEnemyLineOfSite : WeightModule
{
    [SerializeField] float height_offset = 0.1f;
    [SerializeField] float enemy_max_check_distance = 100;


    public override int AssessWeight(TacticalAssessor.WeightedPoint _point, float _area_radius,
        ref List<TacticalAssessor.WeightedPoint> _weighted_positions)
    {
        return VisibleToEnemy(_point.position) ? weight_penalty : 0;
    }


    private bool VisibleToEnemy(Vector3 _position)
    {
        GameObject closest_enemy = FindClosestEnemy(_position);

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
        float dist = Vector2.Distance(ray_origin, target);

        return Physics.Raycast(ray_origin, direction, dist);//if we hit something return false
    }


    public GameObject FindClosestEnemy(Vector3 _position)
    {
        GameObject closest_enemy = null;
        List<GameObject> enemies = GameManager.scene_refs.enemy_manager.enemies;
        float closest_distance = float.PositiveInfinity;

        foreach (var enemy in enemies)
        {
            float dist = (enemy.transform.position - _position).sqrMagnitude;
            if (dist >= closest_distance || dist > enemy_max_check_distance)
                continue;

            closest_distance = dist;
            closest_enemy = enemy;
        }

        return closest_enemy;
    }

}
