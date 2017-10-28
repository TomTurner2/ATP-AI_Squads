using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStateSystem
{
    [Serializable]
    public abstract class Condition : ScriptableObject
    {
        public virtual bool Check(Knowledge _controller)
        {
            return true;
        }
    }
}
