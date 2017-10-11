using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeightModule : ScriptableObject
{
    [SerializeField] protected int weight_penalty = 0;
    public abstract int AssessWeight(TacticalAssessor.WeightedPoint _point, float _area_radius,
        ref List<TacticalAssessor.WeightedPoint> _weighted_positions, Faction _requesters_faction);
}
