using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PostProcessing;

public class TacticalAssessor : MonoBehaviour
{
    private const uint POSITION_START_WEIGHT = 100;
    private const int FLANKED_PENALTY = 90;
    private const float HEIGHT_OFFSET = 0.5f;

    struct WeightedPoint
    {
        public Vector3 position { get; set; }
        public uint weight { get; set; }
    }


    public static List<Vector3> FindOptimalCoverInArea(Vector3 _position, float _radius, int _sample_count = 30, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> weighted_positions = SamplePointInRadius(_position, _radius, _sample_count, _area_mask);//select random points on nav

        if (weighted_positions.Count <= 0)//if none sampled leave
            return new List<Vector3>();

        for (int i = 0; i < weighted_positions.Count; ++i)
        {
            WeightedPoint point = weighted_positions[i];
            NavMeshHit hit;

            if(!NavMesh.FindClosestEdge(point.position, out hit, _area_mask))//get closest edge on nav mesh
                continue;

            point.position = hit.position;//set it as position
            weighted_positions[i] = point;
            point.weight = DetermineWeight(point);//calculate weighting
        }

        weighted_positions.OrderByDescending(p => p.weight);

        List<Vector3> positions = new List<Vector3>(weighted_positions.Select(p => p.position)); ;
        return positions;
    }


    private static List<WeightedPoint> SamplePointInRadius(Vector3 _position, float _radius, int _sample_count = 30, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> positions = new List<WeightedPoint>();

        for (int i = 0; i < _sample_count; ++i)
        {
            Vector3 random_point = _position + Random.insideUnitSphere * _radius;
            NavMeshHit hit;

            if (!NavMesh.SamplePosition(random_point, out hit, 1.0f, _area_mask))//if hit nav mesh
                continue;

            WeightedPoint point = new WeightedPoint//create struct
            {
                position = hit.position,
                weight = POSITION_START_WEIGHT
            };

            positions.Add(point);//add position to list
        }

        return positions;
    }


    private static uint DetermineWeight(WeightedPoint _point)
    {
        int weight = (int) _point.weight;
        
        if (VisibleToEnemy(_point.position))
            weight -= FLANKED_PENALTY;

        if (weight < 0)
            weight = 0;

        return (uint) weight;
    }


    private static bool VisibleToEnemy(Vector3 _position)
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


        //TODO cast from enemy and check if hit anything, if not return true

        return false;
    }


    public static GameObject FindClosestEnemy(Vector3 _position)
    {
        GameObject closest_enemy = null;
        List<GameObject> enemies = EnemyManager.enemies;
        float closest_distance = float.PositiveInfinity;

        foreach (var enemy in enemies)
        {
            float dist = (enemy.transform.position -_position).sqrMagnitude;
            if (dist >= closest_distance)
                continue;

            closest_distance = dist;
            closest_enemy = enemy;
        }

        return closest_enemy;
    }

}
