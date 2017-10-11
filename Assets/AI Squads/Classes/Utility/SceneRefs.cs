using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SceneRefs
{
    private FactionManager faction_manager_ref;
    private TacticalAssessor tactical_assessor_ref;


    public FactionManager FactionManager
    {
        get
        {
            if (faction_manager_ref == null)
                faction_manager_ref = GameObject.FindObjectOfType<FactionManager>();

            return faction_manager_ref;
        }
    }



    public TacticalAssessor tactical_assessor
    {
        get
        {
            if (tactical_assessor_ref == null)
                tactical_assessor_ref = GameObject.FindObjectOfType<TacticalAssessor>();

            return tactical_assessor_ref;
        }
    }
}
