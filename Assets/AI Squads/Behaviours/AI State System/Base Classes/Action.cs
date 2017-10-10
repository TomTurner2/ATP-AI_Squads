using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateSystem
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(MonoBehaviour _controller);
    }
}
