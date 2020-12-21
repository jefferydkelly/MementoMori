using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKnight : Enemy
{
    [SerializeField]
    GameObject sword;

    protected override void Init()
    {
        base.Init();
        state = EnemyState.Move;
        ChooseRandomDirection();
    }

    void Update()
    {
        if (GameManager.Instance.IsInGame)
        {
            if (state == EnemyState.Move)
            {

                Move(Time.deltaTime);
            }
            else if (state == EnemyState.Attack)
            {
                sword.transform.Rotate(Vector3.forward, -90 * Time.deltaTime);
            }
        } 
    }
    protected override void ChangeState()
    {
        if (state == EnemyState.Move)
        {
            state = EnemyState.Attack;
            Attack();
            Timer secondSwipeTimer = new Timer(1);
            secondSwipeTimer.OnComplete.AddListener(Attack);
            secondSwipeTimer.Start();
        } else if (state == EnemyState.Attack)
        {
            state = EnemyState.Idle;
            HideSword();
        } else
        {
            state = EnemyState.Move;
            ChooseRandomDirection();
        }
    }

    void Attack()
    {
        sword.SetActive(true);
        sword.transform.localPosition = forward;
        sword.transform.localRotation = Quaternion.AngleAxis(-45, Vector3.up);
        
    }

    void HideSword()
    {
        sword.SetActive(false);
    }
}
