using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeTheme : MonoBehaviour
{
    [SerializeField] private Button btnThemeFish;
    [SerializeField] private Button btnThemeFruit;
    
    private void Awake()
    {
        btnThemeFish.onClick.AddListener(OnChangeThemeFish);
        btnThemeFruit.onClick.AddListener(OnChangeThemeFruit);
    }

    void OnChangeThemeFish()
    {
        ManagerTheme.Instance.SetThemeByType(Theme.Fish);
    }

    void OnChangeThemeFruit()
    {
        ManagerTheme.Instance.SetThemeByType(Theme.Fruit);
    }
}
