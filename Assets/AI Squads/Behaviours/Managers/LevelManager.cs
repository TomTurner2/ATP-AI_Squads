using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void GameOver()
    {
        GameManager.TriggerGameOver();
    }


    public void PlayAgain()
    {
        GameManager.PlayAgain();
    }


    public void ReturnToMenu()
    {
        GameManager.ReturnToMainMenu();
    }

}
