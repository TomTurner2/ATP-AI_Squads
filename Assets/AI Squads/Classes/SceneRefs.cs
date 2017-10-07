using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SceneRefs
{
    private EnemyManager enemy_manager_ref;


    public EnemyManager enemy_manager
    {
        get
        {
            if (enemy_manager_ref == null)
                enemy_manager_ref = GameObject.FindObjectOfType<EnemyManager>();

            return enemy_manager_ref;
        }
    }
}
