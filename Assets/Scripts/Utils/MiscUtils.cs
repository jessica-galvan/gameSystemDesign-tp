using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiscUtils
{
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

}

public class RandomWeight<T> where T : IWeight
{
    public static T Run(List<T> items, out int index) 
    {
        index = 0;
        int total = 0;
        foreach (var item in items)
        {
            total += item.Weight;
        }

        int random = Random.Range(0, total + 1);

        for (int i = 0; i < items.Count; i++)
        {
            random -= items[i].Weight; //El valor de random - el primer item
            if (random > 0) continue; //Si la resta de esos dos numeros da menor a cero, entonces estamos parados ahi

            index = i;
            return items[i];
        }

        return default(T);
    }
}

public interface IWeight
{
    int Weight { get; }
}