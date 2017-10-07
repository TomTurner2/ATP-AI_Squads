using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*Bit mask helper from:
 https://stackoverflow.com/questions/3261451/using-a-bitmask-in-c-sharp
*/
public static class BitmaskHelper
{
    public static bool IsSet<T>(T _flags, T _flag) where T : struct
    {
        int flags_value = (int)(object)_flags;
        int flag_value = (int)(object)_flag;

        return (flags_value & flag_value) != 0;
    }


    public static void Set<T>(ref T _flags, T _flag) where T : struct
    {
        int flags_value = (int)(object)_flags;
        int flag_value = (int)(object)_flag;

        _flags = (T)(object)(flags_value | flag_value);
    }


    public static void Unset<T>(ref T _flags, T _flag) where T : struct
    {
        int flags_value = (int)(object)_flags;
        int flag_value = (int)(object)_flag;

        _flags = (T)(object)(flags_value & (~flag_value));
    }
}
