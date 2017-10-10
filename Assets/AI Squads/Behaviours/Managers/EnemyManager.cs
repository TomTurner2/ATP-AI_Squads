using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public GameObject FindClosestEnemy(Vector3 _position, float _enemy_max_check_distance = Mathf.Infinity)
    {
        GameObject closest_enemy = null;
        float closest_distance = float.PositiveInfinity;

        foreach (var enemy in enemies)
        {
            float dist = (enemy.transform.position - _position).sqrMagnitude;
            if (dist >= closest_distance || dist > _enemy_max_check_distance)
                continue;

            closest_distance = dist;
            closest_enemy = enemy;
        }

        return closest_enemy;
    }
}
