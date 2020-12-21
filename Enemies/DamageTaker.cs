using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageTaker : MonoBehaviour
{
    public int maxHP = 4;
    int currentHP;

    bool isInvincible = false;
    Timer invincibilityTimer;

    public Alignment alignment = Alignment.Neutral;

    public HealthEvent OnHealthChanged = new HealthEvent();
    public UnityEvent OnDeath = new UnityEvent();

    private void Awake()
    {
        currentHP = maxHP;
       
        OnHealthChanged.AddListener((int hp) =>
        {
            if (hp <= 0)
            {
                OnDeath.Invoke();
            }
        });

        OnDeath.AddListener(Die);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int dmg, GameObject src = null)
    {
        if (!isInvincible && currentHP > 0)
        {
            currentHP -= dmg;
            OnHealthChanged.Invoke(currentHP);
            AudioManager.Instance.PlayClip("Player", "Damage");

            if (src)
            {
                Vector3 dif = src.transform.position - gameObject.transform.position;
                transform.position -= dif.normalized * 0.25f;
            }
        }
    }

    public virtual void Heal(int hp)
    {
        currentHP = Mathf.Min(currentHP + hp, maxHP);
        OnHealthChanged.Invoke(currentHP);
    }

    public void BecomeInvincible(float time)
    {
        isInvincible = true;
        invincibilityTimer = new Timer(time);
        invincibilityTimer.OnComplete.AddListener(() => { isInvincible = false; });
        invincibilityTimer.Start();
    }
}

public enum DamageTypes
{
    Sword,
    Bomb,
    Fire,
    Arrow
}

public class HealthEvent:UnityEvent<int>
{

}