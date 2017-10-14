using System.Collections;
using System.Collections.Generic;


public static class IListExtensions
{
    public static void Shuffle<T>(this IList<T> _list)
    {
        var count = _list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = _list[i];
            _list[i] = _list[r];
            _list[r] = tmp;
        }
    }


    public static void Swap<T>(this IList<T> _list, int _index_a, int _index_b)
    {
        T tmp = _list[_index_a];
        _list[_index_a] = _list[_index_b];
        _list[_index_b] = tmp;
    }
}