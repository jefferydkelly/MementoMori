using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage;
    public Alignment alignment = Alignment.Neutral;
    [SerializeField]
    bool destroyOnImpact = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoDamage(collision.gameObject);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DoDamage(collision.gameObject);
    }

    void DoDamage(GameObject other)
    {
        DamageTaker damaged = other.GetComponent<DamageTaker>();
        if (damaged && damaged.alignment != alignment)
        {
            damaged.TakeDamage(damage, gameObject);
        }

        if (destroyOnImpact)
        {
            Destroy(gameObject);
        }
    }
}

public enum Alignment
{
    Enemy,
    Neutral,
    Player
}
