using System;
//using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URandom = UnityEngine.Random;

public static class Utils
{
    private static readonly List<NormalItem.eNormalType> listFilteredNormalType;
    private static readonly NormalItem.eNormalType[] arrNormalType;
    static Utils()
    {
        Array values = Enum.GetValues(typeof(NormalItem.eNormalType));
        arrNormalType = new NormalItem.eNormalType[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            arrNormalType[i] = (NormalItem.eNormalType)values.GetValue(i);
        }
        
        listFilteredNormalType = new List<NormalItem.eNormalType>();
    }
    public static NormalItem.eNormalType[] GetAllNormalType()
    {
        return arrNormalType;
    }
    
    public static NormalItem.eNormalType GetRandomNormalType()
    {
        Array values = Enum.GetValues(typeof(NormalItem.eNormalType));
        NormalItem.eNormalType result = (NormalItem.eNormalType)values.GetValue(URandom.Range(0, values.Length));

        return result;
    }

    public static NormalItem.eNormalType GetRandomNormalTypeExcept(NormalItem.eNormalType[] types)
    {
        //List<NormalItem.eNormalType> list = Enum.GetValues(typeof(NormalItem.eNormalType)).Cast<NormalItem.eNormalType>().Except(types).ToList();

        // int rnd = URandom.Range(0, list.Count);
        // NormalItem.eNormalType result = list[rnd];
        //
        // return result;
        listFilteredNormalType.Clear();
        
        for (int i = 0; i < arrNormalType.Length; i++)
        {
            bool excluded = false;
            for (int j = 0; j < types.Length; j++)
            {
                if (arrNormalType[i] == types[j])
                {
                    excluded = true;
                    break;
                }
            }

            if (!excluded)
            {
                listFilteredNormalType.Add(arrNormalType[i]);
            }
        }
        
        if (listFilteredNormalType.Count == 0)
        {
            return arrNormalType[URandom.Range(0, arrNormalType.Length)];
        }

        return listFilteredNormalType[URandom.Range(0, listFilteredNormalType.Count)];
    }
}
