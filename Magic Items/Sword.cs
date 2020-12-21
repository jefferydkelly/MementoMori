using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    float activeTime = 1.0f;
    Timer activeTimer;

    [SerializeField]
    float rechargeTime = 0.5f;
    Timer rechargeTimer;

    bool canUse = true;

    private void Awake()
    {
        activeTimer = new Timer(activeTime);
        activeTimer.OnComplete.AddListener(() =>
        {
            rechargeTimer.Start();
            gameObject.SetActive(false);
        });

        rechargeTimer = new Timer(rechargeTime);
        rechargeTimer.OnComplete.AddListener(() =>
        {
            canUse = true;
        });
    }

    public void Swing()
    {
        if (canUse)
        {
            canUse = false;
            gameObject.SetActive(true);
            activeTimer.Start();
        }
    }
}
