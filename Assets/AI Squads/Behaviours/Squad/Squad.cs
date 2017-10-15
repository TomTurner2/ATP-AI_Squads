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
        squad_members.ForEach(SetupNewSquadMember);
    }


    void SetupNewSquadMember(AIController squad_member)
    {
        squad_member.knowledge.stick_to_cover = stick_to_cover;
        RegisterDeathEvent(squad_member.gameObject);
    }


    void RegisterDeathEvent(GameObject _killable_object)
    {
        LifeForce life_force = _killable_object.GetComponent<LifeForce>() ??
            _killable_object.GetComponentInChildren<LifeForce>();

        if (life_force == null)
            return;

        life_force.on_death_event.AddListener(OnSquadMemberDeath);
    }


    public void OnSquadMemberDeath(GameObject _death_game_object)
    {
        AIController last_formation_leader = ai_formation_leader;

        SetFormation(current_formation);

        //if last leader died the new leader should continue to the waypoint
        if (_death_game_object == last_formation_leader.gameObject)
        {
            ai_formation_leader.knowledge.waypoint = last_formation_leader.knowledge.waypoint;
        }
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
        ResetSquadMemberFollowTargets();
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
        int targets_required = GetAliveSquadMemberCount();

        if (follow_commander)
            ++targets_required;

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
        int count = 1;//first position is reserved for formation lead

        foreach (AIController squad_member in squad_members)
        {
            if (squad_member.controlled_character.dead)
                continue;

            if (squad_member.gameObject == current_formation_leader.gameObject)
                continue;

            if (count > _targets.Count - 1)
                break;

            squad_member.knowledge.follow_target = _targets[count];
            ++count;
        }
    }


    void SetFormationLeader(List<Transform> _targets)
    {
        ResetSquadMemberFollowTargets();

        if (follow_commander)
        {
            current_formation_leader = squad_commander.gameObject;
        }
        else
        {
            ai_formation_leader = GetFirstAliveSquadMember();//assign formation leader
            ai_formation_leader.knowledge.follow_target = null;
            current_formation_leader = ai_formation_leader.gameObject;//store ai leader
        }

        _targets[0].position = current_formation_leader.transform.position;
        FollowFormationLeader follow = _targets[0].gameObject.AddComponent<FollowFormationLeader>();//make formation lead point follow formation leader
        follow.InitFollow(current_formation_leader.transform);
    }


    AIController GetFirstAliveSquadMember()
    {
        if (squad_members.Count <= 0)
            return null;

        foreach (AIController squad_member in squad_members)
        {
            if (squad_member.controlled_character.dead)
                continue;

            return squad_member;
        }

        return squad_members[0];
    }


    int GetAliveSquadMemberCount()
    {
        return squad_members.Count(s => !s.controlled_character.dead);
    }
    

    void DistributeWaypointsToSquad(List<Vector3> _waypoints)
    {
        if (_waypoints == null)
            return;

        for (int i = 0; i < squad_members.Count; ++i)
        {
            if (squad_members[i].controlled_character.dead)
                continue;

            if (i >= _waypoints.Count)
                return;

            squad_members[i].knowledge.waypoint = _waypoints[i];
        }
    }
}
