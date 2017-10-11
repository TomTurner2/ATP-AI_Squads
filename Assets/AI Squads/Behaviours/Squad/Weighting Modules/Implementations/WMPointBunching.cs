using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Weight Module/WMPointBunching")]
public class WMPointBunching : WeightModule
{
    [SerializeField] float cover_spacing = 0.3f;

    public override int AssessWeight(TacticalAssessor.WeightedPoint _point, float _area_radius,
        ref List<TacticalAssessor.WeightedPoint> _weighted_positions, Faction _requesters_faction)
    {
        return (CullBunchedPoints(ref _weighted_positions, _point) ? weight_penalty : 0);
    }


    private bool CullBunchedPoints(ref List<TacticalAssessor.WeightedPoint> _weighted_positions,
        TacticalAssessor.WeightedPoint _point)
    {
        int point_index = _weighted_positions.IndexOf(_point);
        ++point_index;

        for (int i = point_index; i < _weighted_positions.Count; ++i)
        { 
            if (_point.original_position == _weighted_positions[i].original_position)
                continue;

            var dist = (_point.position - _weighted_positions[i].position).magnitude;
            if (dist <= cover_spacing)
                return true;
        }

        return false;
    }
}