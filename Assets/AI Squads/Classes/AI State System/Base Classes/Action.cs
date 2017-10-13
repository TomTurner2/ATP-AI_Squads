using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIStateSystem
{
    public abstract class Action : ScriptableObject
    {
        [SerializeField] List<Condition> conditional_requirements = new List<Condition>();
        public abstract void Execute(MonoBehaviour _controller);

        public bool CanExecute(MonoBehaviour _controller)
        {
            if (conditional_requirements.Count <= 0)
                return true;

            //if any of the conditions aren't satisfied this action can't be executed
            return !conditional_requirements.Any(c => c.Check(_controller) == false);
        }
    }
}
