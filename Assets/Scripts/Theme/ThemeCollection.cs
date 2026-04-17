using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Theme
{
    Fruit,
    Fish
}

[CreateAssetMenu(
    fileName = "CollectionTheme",
    menuName = "ScriptableObjects/Theme/CollectionTheme",
    order = 2)]
public class ThemeCollection : ScriptableObject
{
    public NormalItemTheme[] ArrThemes;


    public NormalItemTheme GetDefaultTheme()
    {
        if (ArrThemes != null && ArrThemes.Length > 0) return ArrThemes[0];
        return null;
    }

    public NormalItemTheme GetByType(Theme type)
    {
        if (ArrThemes == null) return null;

        for (int i = 0; i < ArrThemes.Length; i++)
        {
            if (ArrThemes[i] != null && ArrThemes[i].Theme == type)
            {
                return ArrThemes[i];
            }
        }

        return null;
    }

    public int IndexOf(NormalItemTheme theme)
    {
        if (ArrThemes == null || theme == null) return -1;

        for (int i = 0; i < ArrThemes.Length; i++)
        {
            if (ArrThemes[i] == theme) return i;
        }

        return -1;
    }
}