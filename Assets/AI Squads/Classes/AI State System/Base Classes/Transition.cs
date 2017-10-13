using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace AIStateSystem
{
    [System.Serializable]
    public enum TransitionType
    {
        STANDARD,
        PUSH,
        POP
    }

    [System.Serializable]
    public class Transition
    {
        public TransitionType transition_type;
        public Condition condition;
        public State triggered_state;
    }
}
