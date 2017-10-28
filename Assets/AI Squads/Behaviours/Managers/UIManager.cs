using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameplay_ui_canvas;
    [SerializeField] GameObject game_over_ui_canvas;


    public void EnableGameplayUI(bool _enabled)
    {
        gameplay_ui_canvas.SetActive(_enabled);
    }


    public void EnableGameOverUI(bool _enabled)
    {
        game_over_ui_canvas.SetActive(_enabled);
    }
}
