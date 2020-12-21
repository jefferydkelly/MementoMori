using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    Image itemImage;

    public void Awake()
    {
        itemImage = GetComponentsInChildren<Image>()[1];
        SetSprite(itemImage.sprite);
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            itemImage.enabled = false;
        }
        else
        {
            itemImage.enabled = true;
            itemImage.sprite = sprite;
        }
    }
}
