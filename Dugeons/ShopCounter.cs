using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCounter : InteractiveObject
{
    ItemInfo item;

    public void SetItem(ItemInfo info)
    {
        item = info;
        GameObject gameObject = Instantiate(item.item);
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.back;
    }

    protected override void SetAsCurrent()
    {
        base.SetAsCurrent();
        UIManager.Instance.ShowItemInfo(item);
    }

    protected override void RemoveAsCurrent()
    {
        base.RemoveAsCurrent();
        UIManager.Instance.HideItemInfo();
    }

    public override void Interact()
    {
        if (GameManager.Instance.Player.Coins >= item.basePrice)
        {
            Debug.Log("You can buy this");
        } else
        {
            Debug.Log("Too expensive");
        }
    }

}
