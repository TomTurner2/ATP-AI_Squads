using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projector))]
public class ProjectorScaler : MonoBehaviour
{
    [SerializeField] Projector projector_target = null;

    void Start()
    {
        if (projector_target != null)
            return;

        projector_target = GetComponent<Projector>();
    }


    public void SetOrthographicSize(float _size)
    {
        if (projector_target == null)
            return;

        projector_target.orthographicSize = _size;
    }
}
