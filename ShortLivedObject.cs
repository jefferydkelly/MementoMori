using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortLivedObject : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 5.0f;
    Timer lifeTimer;

    private void Awake()
    {
        lifeTimer = new Timer(lifeTime);
        lifeTimer.OnComplete.AddListener(() =>
        {
            Destroy(gameObject);
        });

        lifeTimer.Start();
    }
}
