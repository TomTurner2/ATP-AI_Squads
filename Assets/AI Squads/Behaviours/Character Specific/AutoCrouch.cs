using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[FlagsAttribute]
public enum Directions
{
    NONE = 0,
    FORWARD_UP = 1,
    BACKWARD_UP = 2,
    LEFT_UP = 4,
    RIGHT_UP = 8,
    FORWARD_DOWN = 16,
    BACKWARD_DOWN = 32,
    LEFT_DOWN = 64,
    RIGHT_DOWN = 128,
}

public class AutoCrouch : MonoBehaviour
{
    [SerializeField] CustomEvents.BooleanEvent on_crouch_event;
    private  BitVector32 detections = new BitVector32(0);


    void Start()
    {
        InitBitMask();
    }


    void InitBitMask()
    {
        BitVector32.CreateMask((int)Directions.FORWARD_UP);
        BitVector32.CreateMask((int)Directions.BACKWARD_UP);
        BitVector32.CreateMask((int)Directions.LEFT_UP);
        BitVector32.CreateMask((int)Directions.RIGHT_UP);
        BitVector32.CreateMask((int)Directions.FORWARD_DOWN);
        BitVector32.CreateMask((int)Directions.BACKWARD_DOWN);
        BitVector32.CreateMask((int)Directions.LEFT_DOWN);
        BitVector32.CreateMask((int)Directions.RIGHT_DOWN);
    }


    public void SetFlag(int _direction)
    {
        if (_direction < (int)Directions.NONE || _direction > (int)Directions.RIGHT_DOWN)//!!!!unfortunately unity events don't support enums :(
            return;

        detections[_direction] = true;
    }


    public void UnSetFlag(int _direction)
    {
        if (_direction < (int)Directions.NONE || _direction > (int)Directions.RIGHT_DOWN)
            return;

        detections[_direction] = false;
    }


    void Update()
    {
        bool crouch = CheckDetection(detections, Directions.FORWARD_UP, Directions.FORWARD_DOWN) ||
                      CheckDetection(detections, Directions.BACKWARD_UP, Directions.BACKWARD_DOWN) ||
                      CheckDetection(detections, Directions.LEFT_UP, Directions.LEFT_DOWN) ||
                      CheckDetection(detections, Directions.RIGHT_UP, Directions.RIGHT_DOWN);//if any are true we should crouch

        on_crouch_event.Invoke(crouch);
    }


    private bool CheckDetection(BitVector32 _detected, Directions _direction_up, Directions _direction_down)
    {
        return !_detected[(int) _direction_up] && _detected[(int) _direction_down];//if not top detection but bottom detection
    }

}
