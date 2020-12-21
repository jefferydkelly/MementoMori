using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStaff : MagicItem
{
    Projectile flame;

    float cooldown = 1.0f;
    Timer cooldownTimer;
    bool isOnCooldown = false;

    public FlameStaff() 
    {
        Type = InventoryItems.Staff;
        UISprite = DatabaseManager.Instance.GetSpriteFromDatabase("Items", "Treasures", "Staff", "UI Sprite");
        Description = DatabaseManager.Instance.GetStringFromDatabase("Items", "Treasures", "Staff", "Description");
        flame = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Treasures", "Staff", "Fireball").GetComponent<Projectile>();
        ItemName = "Staff of Flames";


        cooldownTimer = new Timer(cooldown);
        cooldownTimer.OnTick.AddListener(() => { isOnCooldown = false; });
    }

    public override void Activate()
    {
        if (!IsActive && !isOnCooldown)
        {
            IsActive = true;
            Projectile pro = GameObject.Instantiate(flame);
            Vector3 direction = GameManager.Instance.Player.Facing;
            pro.transform.position = GameManager.Instance.Player.transform.position + direction;
            pro.transform.rotation = Quaternion.AngleAxis(direction.GetXY().Angle().ToDegrees() - 90, Vector3.forward);
            pro.Launch(direction);
            isOnCooldown = true;
            cooldownTimer.Start();
        }
    }
}
