using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    PickupType type;

    bool pickedUp = false;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            pickedUp = true;
            GameManager.Instance.Player.PickupItem(this);
            gameObject.SetActive(false);
        }
    }

    public PickupType Type
    {
        get
        {
            return type;
        }
    }
}

public enum PickupType
{
    HealthRefill,
    HealthIncrease,
    Money,
    Item,
    Key,
    Arrow,
    Bomb,
    GoddessCoin
}
