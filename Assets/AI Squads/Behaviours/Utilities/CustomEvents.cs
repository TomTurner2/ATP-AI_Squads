using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEvents
{
    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [System.Serializable]
    public class BooleanEvent : UnityEvent<bool> { }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [System.Serializable]
    public class DamageEvent : UnityEvent<int, RaycastHit> { }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [System.Serializable]
    public class AreaWaypointEvent : UnityEvent<Vector3, float> { }

    [System.Serializable]
    public class SpriteSwapEvent : UnityEvent<Sprite> { }

    [System.Serializable]
    public class AIControllerEvent : UnityEvent<AIController> { }
}
