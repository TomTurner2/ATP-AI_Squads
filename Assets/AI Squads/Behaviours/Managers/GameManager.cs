﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;//singleton

    public static int score_per_kill = 50;
    public static SceneRefs scene_refs = new SceneRefs();


    public void TriggerGameOver()
    {
        //show game over
        scene_refs.ui_manager.EnableGameplayUI(false);
        scene_refs.ui_manager.EnableGameOverUI(true);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ReturnToMainMenu()
    {
        Cursor.visible = true;
        SceneManager.LoadScene(0);
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
