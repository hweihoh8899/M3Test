using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTheme : MonoBehaviour
{
    public static ManagerTheme Instance { get; private set; }
    
    public ThemeCollection ThemeCollection;
    public NormalItemTheme CurrentTheme;
    private const string THEME_COLLECTION_PATH = "theme/collectiontheme";

    public Action<NormalItemTheme> OnChangeTheme;
    HashSet<NormalItem> hsNormalItem = new HashSet<NormalItem>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadThemeCollection();
        LoadDefaultTheme();
    }

    void LoadThemeCollection()
    {
        ThemeCollection = Resources.Load<ThemeCollection>(THEME_COLLECTION_PATH);
    }

    private void LoadDefaultTheme()
    {
        if (ThemeCollection == null) return;
        CurrentTheme = ThemeCollection.GetDefaultTheme();
    }

    public void RegisterItem(NormalItem item)
    {
        hsNormalItem.Add(item);
    }

    public void UnRegisterItem(NormalItem item)
    {
        hsNormalItem.Remove(item);
    }
    
    public void ApplyTheme(NormalItemTheme theme)
    {
        CurrentTheme = theme;
        ApplyAllNormalItems();
        if(OnChangeTheme != null) OnChangeTheme(CurrentTheme);
    }
    
    public void SetThemeByType(Theme theme)
    {
        if (ThemeCollection == null || ThemeCollection.ArrThemes == null) return;
        Debug.Log(theme);
        NormalItemTheme resTheme = ThemeCollection.GetByType(theme);
        ApplyTheme(resTheme);
    }

    void ApplyAllNormalItems()
    {
        NormalItem[] arrCache = new NormalItem[hsNormalItem.Count];
        hsNormalItem.CopyTo(arrCache);
        Debug.Log(arrCache.Length);
        for (int i = 0; i < arrCache.Length; i++)
        {
            if (arrCache[i] != null) arrCache[i].ApplyTheme(CurrentTheme);
        }
    }

    
}
