using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateSystem
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(MonoBehaviour _knowledge);
    }
}
