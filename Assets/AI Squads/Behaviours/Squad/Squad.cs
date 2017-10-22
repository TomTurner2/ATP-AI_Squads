using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] Faction squad_faction = null;
    [SerializeField] SquadCommander squad_commander = null;
    [SerializeField] List<AIController> squad_members = new List<AIController>();

    private List<Transform> current_follow_targets = null;
    private AIController ai_formation_leader = null;
    private GameObject current_formation_leader = null;
    private Formation current_formation = null;
    private bool stick_to_cover = false;
    private bool weapons_free = true;
    private Vector3 last_waypoint = Vector3.zero;
    private float last_radius = 4;
    private Vector3 commander_last_position;
    private float dist_before_back_to_follow_formation = 6f;

    [HideInInspector] public bool follow_commander = false;


    private void Start()
    {
        commander_last_position = squad_commander.transform.position;
        last_waypoint = squad_members.First().transform.position;
        squad_members.ForEach(SetupNewSquadMember);
        SetSquadAreaWaypoint(last_waypoint, 4);
    }


    private void Update()
    {
        HandleFormationBreakForCover();
    }


    private bool CheckCommanderHasMoved()
    {
        Vector3 move = squad_commander.transform.position - commander_last_position;
        float dist = move.magnitude;

        if (dist >= dist_before_back_to_follow_formation)
        {
            commander_last_position = squad_commander.transform.position;
            return true;
        }

        return false;
    }


    private void HandleFormationBreakForCover()
    {
        if (!stick_to_cover)
            return;

        if (follow_commander)
        {
            FormationBreakFromFollowingCommander();
        }
        else
        {
            if (ai_formation_leader == null)
                return;

            FormationBreakFromWaypoint();
        }
    }


    private void FormationBreakFromWaypoint()
    {
        if (ai_formation_leader.nav_mesh_agent.remainingDistance <
            ai_formation_leader.knowledge.target_arrival_tolerance)
        {
            squad_members.ForEach(s => s.knowledge.can_take_cover = true);
        }
        else
        {
            squad_members.ForEach(s => s.knowledge.can_take_cover = false);
        }
    }


    private void FormationBreakFromFollowingCommander()
    {
        AIController squad_member = GetFirstAliveSquadMember();

        if (!CheckCommanderHasMoved())
        {
            DistributeCoverToSquad(GameManager.scene_refs.tactical_assessor.
                FindOptimalCoverInArea(squad_commander.transform.position, last_radius, squad_faction));

            if(SquadMembersAtWaypoint())
                squad_members.ForEach(s => s.knowledge.can_take_cover = true);
        }
        else
        {          
            squad_members.ForEach(s => s.knowledge.can_take_cover = false);
        }
    }


    private bool SquadMembersAtWaypoint()
    {
        foreach (AIController squad_member in squad_members)
        {
            if (squad_member.controlled_character.dead)
                continue;

            if (squad_member.nav_mesh_agent.remainingDistance > squad_member.knowledge.target_arrival_tolerance)
                return false;
        }
        return true;
    }


    private void SetupNewSquadMember(AIController squad_member)
    {
        squad_member.knowledge.can_take_cover = stick_to_cover;
        squad_member.knowledge.can_fire = weapons_free;
        RegisterDeathEvent(squad_member.gameObject);
    }


    private void RegisterDeathEvent(GameObject _killable_object)
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

        if (last_formation_leader == null)
            return;

        if (_death_game_object == last_formation_leader.gameObject)
        {
            ai_formation_leader.knowledge.waypoint = last_formation_leader.knowledge.waypoint;
        }
    }


    public void SetSquadAreaWaypoint(Vector3 _waypoint_pos, float _radius)
    {
        last_waypoint = _waypoint_pos;
        last_radius = _radius;

        DistributeCoverToSquad(GameManager.scene_refs.tactical_assessor.
            FindOptimalCoverInArea(_waypoint_pos, _radius, squad_faction));

        if (current_formation != null)
            SetFormation(current_formation);

        if (current_follow_targets == null || current_follow_targets.Count <= 0)
        {
            HandleNoFormation(_waypoint_pos, _radius);
            return;
        }

        if (ai_formation_leader == null)
            return;

        ai_formation_leader.knowledge.follow_target = null;
        ai_formation_leader.knowledge.waypoint = _waypoint_pos;
    }


    void HandleNoFormation(Vector3 _waypoint_pos, float _radius)
    {
        ClearFormation();

        Vector3 target_pos = _waypoint_pos;

        if (follow_commander)
        {
            target_pos = squad_commander.transform.position;
        }

        //if no formation and not going to cover, give them a random poisiton in the area to travel to
        foreach (AIController squad_member in squad_members)
        {
            Vector2 random_in_circle = Random.insideUnitCircle;
            squad_member.knowledge.waypoint = target_pos + (new Vector3(random_in_circle.x, 0, random_in_circle.y ) * _radius);
        }
    }


    public void SetFormation(Formation _formation)
    {
        InitFormation(_formation);
        
        current_follow_targets = _formation.GetFormation(GetTargetRequirement());

        if (current_follow_targets == null || current_follow_targets.Count <= 0)
        {
            HandleNoFormation(last_waypoint, last_radius);
            return;
        }

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


    public bool ToggleStickToCover()
    {
        stick_to_cover = !stick_to_cover;
        squad_members.ForEach(s => s.knowledge.can_take_cover = stick_to_cover);
        SetFormation(current_formation);

        if (ai_formation_leader)
            ai_formation_leader.knowledge.waypoint = last_waypoint;

        return stick_to_cover;
    }


    public bool ToggleWeaponsFreeCover()
    {
        weapons_free = !weapons_free;
        squad_members.ForEach(s => s.knowledge.can_fire = weapons_free);
        return weapons_free;
    }


    private void DistributeFollowTargetsToSquad(List<Transform> _targets)
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


    private void SetFormationLeader(List<Transform> _targets)
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


    private AIController GetFirstAliveSquadMember()
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


    private int GetAliveSquadMemberCount()
    {
        return squad_members.Count(s => !s.controlled_character.dead);
    }
    

    private void DistributeCoverToSquad(List<Vector3> _waypoints)
    {
        if (_waypoints == null)
            return;

        for (int i = 0; i < squad_members.Count; ++i)
        {
            if (squad_members[i].controlled_character.dead)
                continue;

            if (i >= _waypoints.Count)
                return;

            squad_members[i].knowledge.best_cover = _waypoints[i];
        }
    }

}
