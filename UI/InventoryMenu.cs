using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField]
    List<InventorySlot> inventorySlots;

    private void OnEnable()
    {
        int numItems = GameManager.Instance.Player.Inventory.Count;
        for (int i = 0; i < numItems; i++)
        {
            inventorySlots[i].enabled = true;
            inventorySlots[i].item = GameManager.Instance.Player.Inventory[i];
            inventorySlots[i].Sprite = GameManager.Instance.Player.Inventory[i].UISprite;
        }

        for (int i = numItems; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].enabled = false;
        }

        if (numItems > 0)
        {
            inventorySlots[0].Select();
        }
    }
}
