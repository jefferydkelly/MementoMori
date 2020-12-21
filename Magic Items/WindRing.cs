using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRing : MagicItem
{
    Projectile whirlwind;

    float cooldown = 1.0f;
    Timer cooldownTimer;
    bool isOnCooldown = false;

    public WindRing()
    {
        Type = InventoryItems.Ring;
        UISprite = DatabaseManager.Instance.GetSpriteFromDatabase("Items", "Treasures", "Ring", "UI Sprite");
        whirlwind = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Treasures", "Ring", "Whirlwind").GetComponent<Projectile>();
        Description = DatabaseManager.Instance.GetStringFromDatabase("Items", "Treasures", "Ring", "Description");
        ItemName = "Whirlwind Ring";

        cooldownTimer = new Timer(cooldown);
        cooldownTimer.OnTick.AddListener(() => { isOnCooldown = false; });
    }

    public override void Activate()
    {
        if (!IsActive && !isOnCooldown)
        {
            IsActive = true;
            Projectile pro = GameObject.Instantiate(whirlwind);
            Vector3 direction = GameManager.Instance.Player.Facing;
            pro.transform.position = GameManager.Instance.Player.transform.position + direction;
            pro.transform.rotation = Quaternion.AngleAxis(direction.GetXY().Angle().ToDegrees() - 90, Vector3.forward);
            pro.Launch(direction);
            isOnCooldown = true;
            cooldownTimer.Start();
        }
    }
}
