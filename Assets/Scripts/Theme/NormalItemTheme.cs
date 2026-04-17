using System;
using UnityEngine;
[CreateAssetMenu(
    fileName = "Theme",
    menuName = "ScriptableObjects/Theme/Theme",
    order = 1)]

public class NormalItemTheme : ScriptableObject
{
    public Theme Theme;

    [SerializeField] private ItemData[] arrData;
    
    [Serializable]
    public struct ItemData
    {
        public NormalItem.eNormalType Type;
        public Sprite Sprite;
    }
    
    private void OnEnable()
    {
        BuildStorage();
    }
    
    private System.Collections.Generic.Dictionary<NormalItem.eNormalType, Sprite> dicItem;
    
    private void BuildStorage()
    {
        dicItem = new System.Collections.Generic.Dictionary<NormalItem.eNormalType, Sprite>(7);
        if (arrData == null) return;

        for (int i = 0; i < arrData.Length; i++)
        {
            if (arrData[i].Sprite != null)
            {
                dicItem[arrData[i].Type] = arrData[i].Sprite;
            }
        }
    }
    
    public Sprite GetSprite(NormalItem.eNormalType type)
    {
        if (dicItem == null) BuildStorage();

        dicItem.TryGetValue(type, out Sprite sprite);
        return sprite;
    }

    public bool CheckMissing()
    {
        if (arrData == null || arrData.Length == 0)
        {
            Debug.LogError("Theme has no Sprite");
            return false;
        }
        return true;

        var arrRequired = Enum.GetValues(typeof(NormalItem.eNormalType));
        foreach (NormalItem.eNormalType type in arrRequired)
        {
            bool missing = false;
            for (int i = 0; i < arrData.Length; i++)
            {
                if (arrData[i].Type == type && arrData[i].Sprite != null)
                {
                    missing = true;
                    break;
                }
            }

            if (missing)
            {
                Debug.LogError("Theme missing sprite");
                return false;
            }
        }

        return true;
    }
}
