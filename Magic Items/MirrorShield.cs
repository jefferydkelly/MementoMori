using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorShield : MagicItem
{
    GameObject shield;

    float cooldown = 1.0f;
    Timer cooldownTimer;
    bool isOnCooldown = false;

    public MirrorShield()
    {
        Type = InventoryItems.Shield;
        UISprite = DatabaseManager.Instance.GetSpriteFromDatabase("Items", "Treasures", "Shield", "UI Sprite");
        Description = DatabaseManager.Instance.GetStringFromDatabase("Items", "Treasures", "Shield", "Description");
        ItemName = "Mirror Shield";

        GameObject shieldPrefab = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Treasures", "Shield", "GameObject");
        shield = GameObject.Instantiate(shieldPrefab);
        shield.transform.SetParent(GameManager.Instance.Player.transform);
        shield.SetActive(false);
        cooldownTimer = new Timer(cooldown);
        cooldownTimer.OnTick.AddListener(() => { isOnCooldown = false; });
    }

    public override void Activate()
    {
        if (!isOnCooldown)
        {
            IsActive = true;
            shield.SetActive(true);
            shield.transform.localPosition = GameManager.Instance.Player.Facing * 1.1f;
            GameManager.Instance.Player.Freeze();
        }
    }

    public override void Deactivate()
    {
        if (IsActive)
        {
            shield.SetActive(false);
            IsActive = false;
            isOnCooldown = true;
            cooldownTimer.Start();
            GameManager.Instance.Player.Unfreeze();
        }
    }
}
