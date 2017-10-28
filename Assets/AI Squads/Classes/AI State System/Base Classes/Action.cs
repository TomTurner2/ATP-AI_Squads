using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIStateSystem
{
    [Serializable]
    public abstract class Action : ScriptableObject
    {
        [SerializeField] List<Condition> conditional_requirements = new List<Condition>();

        public virtual void Execute(Knowledge _controller){}

        public bool CanExecute(Knowledge _controller)
        {
            if (conditional_requirements.Count <= 0)
                return true;

            //if any of the conditions aren't satisfied this action can't be executed
            return !conditional_requirements.Any(c => c.Check(_controller) == false);
        }
    }
}
