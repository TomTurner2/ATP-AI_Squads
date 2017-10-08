using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SceneRefs
{
    private EnemyManager enemy_manager_ref;
    private TacticalAssessor tactical_assessor_ref;


    public EnemyManager enemy_manager
    {
        get
        {
            if (enemy_manager_ref == null)
                enemy_manager_ref = GameObject.FindObjectOfType<EnemyManager>();

            return enemy_manager_ref;
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
