using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;//singleton

    public static SceneRefs scene_refs = new SceneRefs();


    public void TriggerGameOver()
    {
        //show game over and score screen

    }


    public void Retry()
    {
        //reload level

    }



    void Awake()
    {
        if (instance == null)
        {
            InitSingleton();
            return;
        }
        Destroy(this.gameObject);
    }


    void InitSingleton()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;   
    }
}
