using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIStateSystem
{
    [CreateAssetMenu(menuName = "AIStateSystem/StateMachine")]
    public class StateMachine : ScriptableObject
    {
        public List<State> states;

        [HideInInspector] public List<State> active_states = new List<State>();

        public void InitStateMachine()
        {
            if (states == null)
                Destroy(this);

            active_states.Add(states.First());
        }


        public void UpdateState(MonoBehaviour _knowledge)
        {
            active_states.Last().UpdateState(this, _knowledge);
        }


        public void SetState(State _new_state)
        {
            active_states.Clear();
            active_states.Add(_new_state);
        }


        public void PushState(State _new_state)
        {
            active_states.Add(_new_state);
        }


        public void PopState()
        {
            active_states.Remove(active_states.Last());
        }
    }
}
