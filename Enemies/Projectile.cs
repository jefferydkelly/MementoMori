using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float movespeed = 2.0f;
    [SerializeField]
    bool destroyOnImpact = true;

    Vector3 direction;
    bool launched = false;
    // Start is called before the first frame update
    private void Awake()
    {
        direction = Vector3.zero;
    }


    public void Launch(Vector3 dir)
    {
        direction = dir;
        launched = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (launched)
        {
            transform.position += direction * movespeed * Time.deltaTime;
        }
    }

    public void Deflect()
    {
        direction *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (launched)
        {
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
            else
            {
                launched = false;
            }
        }
    }
}
