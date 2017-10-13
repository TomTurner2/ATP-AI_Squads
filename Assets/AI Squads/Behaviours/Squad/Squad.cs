using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private Faction squad_faction = null;
    [SerializeField] private SquadCommander squad_commander = null;
    [SerializeField] private List<AIController> squad_members = new List<AIController>();

    private List<Transform> current_follow_targets = null;
    private AIController ai_formation_leader = null;
    private GameObject current_formation_leader = null;
    private Formation current_formation = null;
    public bool follow_commander = false;
    private bool stick_to_cover = false;


    void Start()
    {
        squad_members.ForEach(s => s.knowledge.stick_to_cover = stick_to_cover);
    }
    

    public void SetSquadAreaWaypoint(Vector3 _waypoint_pos, float _radius)
    {
        if (current_formation != null)
            SetFormation(current_formation);

        if (ai_formation_leader == null)
            return;

        ai_formation_leader.knowledge.follow_target = null;
        ai_formation_leader.knowledge.waypoint = _waypoint_pos;
    }


    private void SetOwnWaypoints()
    {
        ClearFormation();
    }


    public void SetFormation(Formation _formation)
    {
        InitFormation(_formation);
        
        current_follow_targets = _formation.GetFormation(GetTargetRequirement());
        if (current_follow_targets == null)
            return;

        if (current_follow_targets.Count <= 0)
            return;
       
        SetFormationLeader(current_follow_targets);
        DistributeFollowTargetsToSquad(current_follow_targets);    
    }


    private void InitFormation(Formation _formation)
    {
        ClearFormation();
        current_formation = _formation;      
    }


    private void ClearFormation()
    {
        current_formation_leader = null;
        ResetSquadMemberFollowTargets();
        ClearOldTargets();
    }


    private int GetTargetRequirement()
    {
        int targets_required = squad_members.Count;

        if (follow_commander)
        {
            targets_required++;
        }

        return targets_required;
    }


    private void ClearOldTargets()
    {
        if (current_follow_targets != null)
            current_follow_targets.ForEach(t => Destroy(t.gameObject));//destroy last targets
    }


    private void ResetSquadMemberFollowTargets()
    {
        if (squad_members.Count <= 0)
            return;

        squad_members.ForEach(s => s.knowledge.follow_target = null);
    }


    public bool ToggleFollowCommander()
    {
        follow_commander = !follow_commander;

        SetFormation(current_formation);

        return follow_commander;
    }


    public void ToggleStickToCover()
    {
        stick_to_cover = !stick_to_cover;
        squad_members.ForEach(s => s.knowledge.stick_to_cover = stick_to_cover);
    }


    void DistributeFollowTargetsToSquad(List<Transform> _targets)
    {
        int count = follow_commander ? 1 : squad_members.FindIndex(s => s.gameObject == current_formation_leader);//if formation leader is not commander make sure that member doesn't follow a target

        foreach (AIController squad_member in squad_members)
        {
            if (count > _targets.Count - 1)
                break;

            squad_member.knowledge.follow_target = _targets[count];
            ++count;
        }
    }


    void SetFormationLeader(List<Transform> _targets)
    {
        current_formation_leader = squad_members[0].gameObject;//assign formation leader
        squad_members[0].knowledge.follow_target = null;
        ai_formation_leader = squad_members.Find(s => s.gameObject == current_formation_leader);

        if (follow_commander)
            current_formation_leader = squad_commander.gameObject;

        _targets[0].position = current_formation_leader.transform.position;
        FollowTransform follow = _targets[0].gameObject.AddComponent<FollowTransform>();
        follow.follow_target = current_formation_leader.transform;
    }


    void DistributeWaypointsToSquad(List<Vector3> _waypoints)
    {
        if (_waypoints == null)
            return;

        for (int i = 0; i < squad_members.Count; ++i)
        {
            if (i < _waypoints.Count)
            {
                squad_members[i].knowledge.waypoint = _waypoints[i];
            }
            else
            {
                return;
            }
        }
    }
}
