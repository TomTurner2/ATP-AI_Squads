using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PostProcessing;
using Random = UnityEngine.Random;

public class TacticalAssessor : MonoBehaviour
{
    [SerializeField] List<WeightModule> weight_modules = new List<WeightModule>();
    [SerializeField] bool debug = false;
    [SerializeField] List<WeightedPoint> debug_last_sample = new List<WeightedPoint>();

    private const int POSITION_START_WEIGHT = 100;

    [System.Serializable]
    public struct WeightedPoint
    {
        public Vector3 position;
        public Vector3 original_position;
        public int weight;
    }


    public List<Vector3> FindOptimalCoverInArea(Vector3 _position, float _radius, float _cover_spacing = 0.3f, int _sample_count = 30,
        int _minimum_score = 0, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> weighted_positions = SampleDistributedPointInRadius(_position,
            _radius, _area_mask);//select random points on nav

        if (weighted_positions.Count <= 0)//if none sampled leave
            return new List<Vector3>();

        FindCoverLocations(ref weighted_positions, _position, _radius, _cover_spacing, _sample_count, _minimum_score, _area_mask);//find cover
        weighted_positions = new List<WeightedPoint>(weighted_positions.OrderByDescending(p => p.weight));
        List<Vector3> positions = new List<Vector3>(weighted_positions.Select(p => p.position));
        debug_last_sample = weighted_positions;
        return positions;
    }


    private void FindCoverLocations(ref List<WeightedPoint> _weighted_positions, Vector3 _position, float _radius, float _cover_spacing = 0.3f,
        int _sample_count = 30, int _minimum_score = 0, int _area_mask = NavMesh.AllAreas)
    {
        for (int i = 0; i < _weighted_positions.Count; ++i)
        {
            WeightedPoint point = _weighted_positions[i];
            NavMeshHit hit;

            if (!NavMesh.FindClosestEdge(point.position, out hit, _area_mask))//get closest edge on nav mesh
                continue;
           
            point.position = hit.position;//set it as position
            _weighted_positions[i] = point;    
        }

        CullBunchedPoints(ref _weighted_positions, _cover_spacing);

        for(int i = 0; i < _weighted_positions.Count; ++i)
        {
            WeightedPoint point = _weighted_positions[i];
            point.weight = DetermineWeight(point, _position, _radius);//calculate weighting
            _weighted_positions[i] = point;
        }
    }


    private void CullBunchedPoints(ref List<WeightedPoint> _weighted_positions, float _cover_spacing = 0.3f)
    {
        foreach (WeightedPoint point_a in _weighted_positions.ToList())
        {
            for (int i = 1; i < _weighted_positions.Count; ++i)
            { 
                if (point_a.original_position == _weighted_positions[i].original_position)
                    continue;

                var dist = (point_a.position - _weighted_positions[i].position).magnitude/*.sqrMagnitude*/;
                if (dist <= _cover_spacing /** _cover_spacing*/)
                {
                    var weighted_position = _weighted_positions[i];
                    weighted_position.weight -= 1000; /*.Remove(_weighted_positions[i]);*/
                    _weighted_positions[i] = weighted_position;
                }
            }
        }
    }


    private List<WeightedPoint> SampleDistributedPointInRadius(Vector3 _position, float _radius, int _area_mask = NavMesh.AllAreas)
    {
        List<WeightedPoint> positions = new List<WeightedPoint>();

        int point_count = 1;
        int point_increase = 2;
        
        float radius_spacing = 0.5f;

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


    private int DetermineWeight(WeightedPoint _point, Vector3 _position, float _radius)
    {
        int weight =  _point.weight;

        foreach (WeightModule module in weight_modules)
        {
            if (module == null)
                continue;

            weight += module.AssessWeight(_point, _radius);
        }

        return  weight;
    }


    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = Color.white;

        if (debug_last_sample == null)
            return;

        foreach (WeightedPoint point in debug_last_sample)
        {
            Gizmos.DrawSphere(point.original_position, 0.05f);
        }
    }

}
