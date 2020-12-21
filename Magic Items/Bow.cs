using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MagicItem
{
    Projectile arrow;

    int ammo = 10;

    public int Ammo
    {
        get
        {
            return ammo;
        }

        set
        {
            ammo = Mathf.Clamp(value, 0, maxAmmo);
        }
    }
    int maxAmmo = 20;
    float cooldown = 1.0f;
    Timer cooldownTimer;
    bool isOnCooldown = false;

    AudioClip loosingFX;

    public Bow()
    {
        arrow = DatabaseManager.Instance.GetGameObjectFromDataBase("Items", "Basic", "Arrow", "GameObject").GetComponent<Projectile>();
        UISprite = DatabaseManager.Instance.GetSpriteFromDatabase("Items", "Treasures", "Bow", "UI Sprite");
        loosingFX = DatabaseManager.Instance.GetClipFromDatabase("Sounds", "FX", "Arrow", "Loose");
        Description = DatabaseManager.Instance.GetStringFromDatabase("Items", "Treasures", "Bow", "Description");
        ItemName = "Hero's Bow";



        cooldownTimer = new Timer(cooldown);
        cooldownTimer.OnTick.AddListener(()=>{ isOnCooldown = false; });
        Type = InventoryItems.Bows;
    }

    public override void Activate()
    {
        if(!IsActive && !isOnCooldown && ammo > 0)
        {
            IsActive = true;
            ammo--;
            AudioManager.Instance.PlaySound(loosingFX);
            Projectile pro = GameObject.Instantiate(arrow);
            Vector3 direction = GameManager.Instance.Player.Facing;
            pro.transform.position = GameManager.Instance.Player.transform.position + direction;
            pro.transform.rotation = Quaternion.AngleAxis(direction.GetXY().Angle().ToDegrees() - 45, Vector3.forward);
            pro.Launch(direction);
            isOnCooldown = true;
            cooldownTimer.Start();
        }
    }
}
