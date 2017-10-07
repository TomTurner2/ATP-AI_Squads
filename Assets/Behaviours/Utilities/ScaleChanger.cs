using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    public void SetSize(Vector3 _size)
    {
        transform.localScale = _size;
    }


    public void SetScale(float _scale = 1)
    {
        transform.localPosition = Vector3.one * _scale;
    }
}
