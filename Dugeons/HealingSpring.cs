using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpring : MonoBehaviour
{
    bool isUsedUp = false;
    Timer healTimer;
    float healTime = 0.25f;


    private void Awake()
    {
        healTimer = new Timer(healTime, true);
        healTimer.OnTick.AddListener(()=> { 
            GameManager.Instance.Player.Heal(1); 
        });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            healTimer.Start();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            healTimer.Stop();
        }
    }
}
