using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PostProcessing;

public class TacticalAssessor : MonoBehaviour
{
    private const int POSITION_START_WEIGHT = 100;
    private const int FLANKED_PENALTY = 1000;
    private const int DISTANCE_FROM_WAYPOINT_PENALTY = 5;
    private const float HEIGHT_OFFSET = 0.1f;

    public bool debug = false;
    private static Vector3 debug_line_start = Vector3.zero;
    private static Vector3 debug_line_end = Vector3.zero;
    private static List<WeightedPoint> debug_last_sample;


    struct WeightedPoint
    {
        public Vector3 position { get; set; }
        public Vector3 original_position { get; set; }
        public int weight { get; set; }
    }


    public static List<Vector3> FindOptimalCoverInArea(Vector3 _position, float _radius, int _sample_count = 30, int _minimum_score = 0, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> weighted_positions = SampleDistributedPointInRadius(_position, _radius, _area_mask);//select random points on nav

        if (weighted_positions.Count <= 0)//if none sampled leave
            return new List<Vector3>();

        for (int i = 0; i < weighted_positions.Count; ++i)
        {
            WeightedPoint point = weighted_positions[i];
            NavMeshHit hit;

            if (!NavMesh.FindClosestEdge(point.position, out hit, _area_mask))//get closest edge on nav mesh
                continue;

            point.position = hit.position;//set it as position
            
            weighted_positions[i] = point;
            point.weight = DetermineWeight(point, _position, _radius);//calculate weighting
        }

        weighted_positions.OrderByDescending(p => p.weight);
        //weighted_positions.Reverse();

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

            if (!NavMesh.SamplePosition(random_point, out hit, 0.1f, _area_mask))//if hit nav mesh
                continue;

            WeightedPoint point = new WeightedPoint//create struct
            {
                original_position = hit.position,
                position = hit.position,
                weight = POSITION_START_WEIGHT
            };

            positions.Add(point);//add position to list
        }

        return positions;
    }


    private static List<WeightedPoint> SampleDistributedPointInRadius(Vector3 _position, float _radius, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> positions = new List<WeightedPoint>();

        int point_count = 1;
        int point_increase = 2;
        
        float radius_spacing = 0.4f;

        for (float r = 0; r < _radius; r += radius_spacing)
        {
            for (int i = 0; i < point_count; i++)
            {
                float theta = (2 * Mathf.PI / point_count * i);
                float x = Mathf.Cos(theta) * r;
                float z = Mathf.Sin(theta) * r;

                NavMeshHit hit;

                if (!NavMesh.SamplePosition(new Vector3(x, _position.y, z) + _position, out hit, 2f, _area_mask))//if hit nav mesh
                    continue;

                WeightedPoint point = new WeightedPoint//create struct
                {
                    original_position = hit.position,
                    position = hit.position,
                    weight = POSITION_START_WEIGHT
                };

                positions.Add(point);//add position to list

            }

            point_count += point_increase;
        }

        debug_last_sample = positions;
        return positions;
    }


    private static int DetermineWeight(WeightedPoint _point, Vector3 _position, float _radius)
    {
        int weight =  _point.weight;
        
        if (VisibleToEnemy(_point.position))
            weight -= FLANKED_PENALTY;

        //factor in distance from commanded position
        if (Vector3.Distance(_position, _point.position) > _radius)
            weight -= DISTANCE_FROM_WAYPOINT_PENALTY;

        return  weight;
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

        Vector3 target = new Vector3(_position.x, _position.y + HEIGHT_OFFSET, _position.z);//offset target from ground
        Vector3 direction = target - ray_origin;
        float dist = Vector2.Distance(ray_origin, target);

        debug_line_start = ray_origin;
        debug_line_end = target;

        return Physics.Raycast(ray_origin, direction, dist/*, 1 << LayerMask.NameToLayer("Enemy")*/);//if we hit something return false
    }


    public static GameObject FindClosestEnemy(Vector3 _position)
    {
        GameObject closest_enemy = null;
        List<GameObject> enemies = GameManager.scene_refs.enemy_manager.enemies;
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


    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        
        Gizmos.DrawLine(debug_line_start, debug_line_end);

        Gizmos.color = Color.white;

        if (debug_last_sample == null)
            return;

        foreach (WeightedPoint point in debug_last_sample)
        {
            Gizmos.DrawSphere(point.original_position, 0.05f);
        }
        
    }

}
