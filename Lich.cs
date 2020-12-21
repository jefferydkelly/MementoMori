using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lich : MonoBehaviour
{
    Vector3 forward = Vector3.down;
    Vector3 travelDirection;
    int rotationDirection = 1;
    float moveSpeed = 3;

    float moveTime = 1.5f;
    Timer moveTimer;

    float pauseTime = 1.0f;
    Timer pauseTimer;

    MovementStyle moveStyle = MovementStyle.Freeze;
    Animator myAnimator;

    [SerializeField]
    Projectile magicAttack;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        moveTimer = new Timer(moveTime);
        
        moveTimer.OnComplete.AddListener(() =>
        {
            FacePlayer();
            travelDirection = Vector3.zero;
            pauseTimer.Start();
        });

        pauseTimer = new Timer(pauseTime);
        pauseTimer.OnComplete.AddListener(() =>
        {
            FacePlayer();
            FireProjectile();

            moveStyle = (MovementStyle)(Random.Range(1, 3));

            if (moveStyle == MovementStyle.Rotate)
            {
                rotationDirection = Random.Range(0, 1) * 2 - 1;
                travelDirection = Vector2.down.RotateDeg(Random.Range(-4, 3) * 45);
                myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(travelDirection));
            } else
            {
                travelDirection = Vector2.down.RotateDeg(Random.Range(0, 4) * 90);
                myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(travelDirection));
            }
            moveTimer.Start();
        });

        
        moveStyle = MovementStyle.Rotate;
    }

    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0, -2);
        moveTimer.Start();
        travelDirection = Vector2.down.RotateDeg(-45);
        FacePlayer();
    }

    private void OnDisable()
    {
        moveTimer.Stop();
    }

    private void OnDestroy()
    {
        moveTimer.Stop();
        pauseTimer.Stop();
    }

    private void Update()
    {
        transform.position += travelDirection * moveSpeed * Time.deltaTime;
        if (moveStyle == MovementStyle.Rotate)
        {
            travelDirection = ((Vector2)(travelDirection)).RotateDeg(120 * rotationDirection *  Time.deltaTime);
        }
    }

    void FireProjectile()
    {
        Projectile proj = Instantiate(magicAttack);
        proj.transform.position = transform.position + forward * 1.5f;
        proj.Launch(forward);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollision();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnCollision();
    }

    void OnCollision()
    {
        if (moveStyle == MovementStyle.Line)
        {
            travelDirection *= -1;
        }
        else
        {
            rotationDirection *= -1;
        }
    }

    void FacePlayer()
    {
        Vector2 dif = GameManager.Instance.Player.transform.position - transform.position;
        forward = dif.normalized;
        myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(dif));
    }
}

public enum MovementStyle
{
    Freeze,
    Line,
    Rotate
}
