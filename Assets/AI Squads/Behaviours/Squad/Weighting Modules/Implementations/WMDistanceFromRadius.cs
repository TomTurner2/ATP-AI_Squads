using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weight Module/WMDistanceFromRadius")]
public class WMDistanceFromRadius : WeightModule
{
    public override int AssessWeight(TacticalAssessor.WeightedPoint _point, float _area_radius,
        ref List<TacticalAssessor.WeightedPoint> _weighted_positions, Faction _requesters_faction)
    {
        //factor in distance from commanded position
        return Vector3.Distance(_point.original_position, _point.position) > _area_radius ? weight_penalty : 0;
    }

}
