using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : InteractiveObject
{
    public override void Interact()
    {
        GameManager.Instance.Player.CarryObject(this);
    }

    private void OnDestroy()
    {
        foreach (Collider2D col in Physics2D.OverlapCircleAll(transform.position, 3.0f))
        {
            col.GetComponent<DamageTaker>()?.TakeDamage(4);
        }
        AudioManager.Instance.PlayClip("Bomb", "Explosion");
    }
}
