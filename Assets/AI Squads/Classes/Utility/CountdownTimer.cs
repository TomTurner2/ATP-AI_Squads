using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CountdownTimer
{
    public float timer_duration { get; set; }
    public float current_time { get; set; }
    public bool auto_reset { get; set; }

    private bool paused;


    public void InitCountDownTimer(float _seconds, bool _auto_reset = true)
    {
        paused = false;
        timer_duration = _seconds;
        current_time = timer_duration;
        auto_reset = _auto_reset;
    }


    public bool UpdateTimer()
    {
        if (paused)//if paused
            return CheckFinished();//return 

        current_time -= Time.deltaTime;//tick down time

        if (CheckFinished())//timer finished
            return false;

        if (auto_reset)
            current_time = timer_duration;//reset automaticaly

        return true;//timer not finished
    }


    public void Pause()
    {
        paused = true;
    }


    public void Start()
    {
        paused = false;
    }


    public bool CheckFinished()
    {
        return !(current_time > 0);
    }


    public void Reset()
    {
        current_time = timer_duration;
    }

}
