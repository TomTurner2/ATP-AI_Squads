using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weight Module/WMDistanceFromRadius")]
public class WMDistanceFromRadius : WeightModule
{
    public override int AssessWeight(TacticalAssessor.WeightedPoint _point, float _radius = 0)
    {
        //factor in distance from commanded position
        if (Vector3.Distance(_point.original_position, _point.position) > _radius)
            return weight_penalty;

        return 0;
    }

}
