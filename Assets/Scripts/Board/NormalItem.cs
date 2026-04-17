using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalItem : Item
{
    public enum eNormalType
    {
        TYPE_ONE,
        TYPE_TWO,
        TYPE_THREE,
        TYPE_FOUR,
        TYPE_FIVE,
        TYPE_SIX,
        TYPE_SEVEN
    }

    public eNormalType ItemType;

    public void SetType(eNormalType type)
    {
        ItemType = type;
    }

    protected override void OnViewSet()
    {
        base.OnViewSet();
        if (ManagerTheme.Instance != null)
        {
            if (ManagerTheme.Instance != null)
            {
                ApplyTheme(ManagerTheme.Instance.CurrentTheme);
                ManagerTheme.Instance.RegisterItem(this);
            }
        }
    }

    protected override void OnViewRelease()
    {
        if (ManagerTheme.Instance != null)
        {
            ManagerTheme.Instance.UnRegisterItem(this);
        }

        base.OnViewRelease();
    }
    
    public void ApplyTheme(NormalItemTheme theme)
    {
        if (theme == null) return;
        if (SpriteRenderer == null) return;
        
        Sprite sprite = theme.GetSprite(ItemType);
        if (sprite != null)
        {
            SpriteRenderer.sprite = sprite;
        }
    }
    

    protected override string GetPrefabName()
    {
        return Constants.PREFAB_NORMAL_ITEM;
    }

    internal override bool IsSameType(Item other)
    {
        NormalItem it = other as NormalItem;

        return it != null && it.ItemType == this.ItemType;
    }
}
