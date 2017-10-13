using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMath 
{
    public static List<Vector3> DistributedPointsInRadius(Vector3 _position, float _radius, int _point_start_count = 1,
        int _point_increase = 4, float _radius_spacing = 0.8f)
    {
        List<Vector3> positions = new List<Vector3>();

        for (float r = 0; r < _radius; r += _radius_spacing)
        {
            for (int i = 0; i < _point_start_count; i++)
            {
                float theta = (2 * Mathf.PI / _point_start_count * i);
                float x = Mathf.Cos(theta) * r;
                float z = Mathf.Sin(theta) * r;

                positions.Add(new Vector3(x, _position.y, z));//add position to list
            }
            _point_start_count += _point_increase;
        }

        return positions;
    }


    public static List<Vector3> PointsOnCircumference(Vector3 _position, float _radius, int _point_count)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < _point_count; ++i)
        {
            float theta = (2 * Mathf.PI / _point_count * i);
            float x = Mathf.Cos(theta) * _radius;
            float z = Mathf.Sin(theta) * _radius;

            positions.Add(new Vector3(x, _position.y, z));//add position to list
        }

        return positions;
    }

}
