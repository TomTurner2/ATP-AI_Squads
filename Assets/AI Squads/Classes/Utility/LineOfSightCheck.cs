using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineOfSightCheck
{
    public static bool CheckLineOfSight(Vector3 _origin, Vector3 _target)
    {
        RaycastHit[] hits = ShootRay(_origin, _target);
        return !CheckHits(hits);//if no hit objects there must be line of sight!
    }


    public static bool CheckLineOfSight(Vector3 _origin, Vector3 _target, Collider _ignore_collider)
    {
        RaycastHit[] hits = ShootRay(_origin, _target);
        return !CheckHits(hits, _ignore_collider);
    }


    public static bool CheckLineOfSight(Vector3 _origin, Vector3 _target, List<Collider> _ignore_colliders)
    {
        RaycastHit[] hits = ShootRay(_origin, _target);
        return !CheckHits(hits, _ignore_colliders);
    }


    private static Vector3 CalculateDirection(Vector3 _origin, Vector3 _target)
    {
        return (_target - _origin).normalized;
    }


    private static float CalculateDistance(Vector3 _origin, Vector3 _target)
    {
        return (_origin - _target).magnitude;
    }


    private static bool CheckHits(RaycastHit[] _hits)
    {
        return _hits.Length > 0;//return true if we hit anything at all
    }


    private static bool CheckHits(RaycastHit[] _hits, Collider _ignore_collider)
    {
        if (_hits.Length <= 0)
            return false;

        return _hits.Any(hit => hit.collider != _ignore_collider);
    }


    private static bool CheckHits(RaycastHit[] _hits, List<Collider> _ignore_colliders)
    {
        if (_hits.Length <= 0)
            return false;

        return _hits.Any(hit => !_ignore_colliders.Contains(hit.collider));//if any hit collider is not in the list
    }


    private static RaycastHit[] ShootRay(Vector3 _origin, Vector3 _target)
    {
        Vector3 dir = CalculateDirection(_origin, _target);
        float dist = CalculateDistance(_origin, _target);

        Ray ray = new Ray(_origin, dir);
        return Physics.RaycastAll(ray, dist);
    }

}
