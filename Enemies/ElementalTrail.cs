using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalTrail : MonoBehaviour
{
    float lifeTime = 1.0f;
    Timer lifeTimer;

    private void Awake()
    {
        lifeTimer = new Timer(lifeTime);
        lifeTimer.OnComplete.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }
}
