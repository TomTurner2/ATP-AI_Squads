using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateSystem
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Check(MonoBehaviour _controller);
    }
}
