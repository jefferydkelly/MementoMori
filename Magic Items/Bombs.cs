using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombs : MagicItem
{
    GameObject bomb;

    int bombs = 10;
    int maxBombs = 10;
    public int Ammo
    {
        get
        {
            return bombs;
        }

        set
        {
            bombs = Mathf.Clamp(value, 0, maxBombs);
        }
    }

    public Bombs()
    {
        bomb = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Basic", "Bomb", "GameObject");
        UISprite = bomb.GetComponent<SpriteRenderer>().sprite;
        ItemName = "Bombs";
        Description = "Press the Item Key to place a bomb.  Press the Interact Key to pick it up and again to throw it.";
        Type = InventoryItems.Bombs;
    }
    public override void Activate()
    {
        if (!IsActive && bombs > 0)
        {
            IsActive = true;
            bombs--;
            GameObject pro = GameObject.Instantiate(bomb);
            pro.transform.position = GameManager.Instance.Player.transform.position + GameManager.Instance.Player.Facing;
        }
    }
}
