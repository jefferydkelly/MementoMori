using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : Button
{
    [HideInInspector]
    public MagicItem item;

    Image image;
    protected override void Awake()
    {
        image = GetComponent<Image>();
        Sprite = null;
        onClick.AddListener(()=> {
            GameManager.Instance.Player.SetSecondaryItem(item);
        });
    }

    public override void OnSelect(BaseEventData eventData)
    {
        ShowDescription();
    }
    public override void OnSubmit(BaseEventData eventData)
    {
        GameManager.Instance.Player.SetSecondaryItem(item);
    }

    public void ShowDescription()
    {
        UIManager.Instance.ShowMessage(item.Description);
    }

    public  override void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowMessage(item.Description, false);
    }

    public Sprite Sprite
    {
        get
        {
            return image.sprite;
        }

        set
        {
            image.sprite = value;

            image.enabled = (value != null);
        }
    }
}
