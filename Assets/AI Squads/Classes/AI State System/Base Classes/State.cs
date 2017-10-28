using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace AIStateSystem
{
    [Serializable][CreateAssetMenu(menuName = "AIStateSystem/State")]
    public class State : ScriptableObject
    {     
        public List<Action> actions = new List<Action>();
        public List<Transition> transitions = new List<Transition>();


        public void UpdateState(StateMachine _state_controller, Knowledge _knowledge)
        {
            ExecuteActions(_knowledge);
            CheckTransitions(_state_controller, _knowledge);
        }


        private void ExecuteActions(Knowledge _knowledge)
        {
            foreach (Action action in actions)
            {
                if (action.CanExecute(_knowledge))
                    action.Execute(_knowledge);
            }
        }


        private void CheckTransitions(StateMachine _state_controller, Knowledge _knowledge)
        {
            foreach (Transition transition in transitions)
            {
                bool decision = transition.condition.Check(_knowledge);

                if (!decision)
                    continue;

                switch (transition.transition_type)
                {
                    case TransitionType.STANDARD:
                        _state_controller.SetState(transition.triggered_state);//set the current state
                        break;
                    case TransitionType.PUSH:
                        _state_controller.PushState(transition.triggered_state);//push the new state on top
                        break;
                    case TransitionType.POP:
                        //if last state is the state the transition wants to pop
                        if (_state_controller.active_states.Last() == transition.triggered_state)
                            _state_controller.PopState();//pop it
                        break;
                }
            }
        }
    }
}
