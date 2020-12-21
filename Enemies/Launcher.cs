using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Enemy
{
    [SerializeField]
    Projectile projectile;

    protected override void ChangeState()
    {
        if (state == EnemyState.Move)
        {
            if (Random.value >= 0.5f)
            {
                state = EnemyState.Attack;
            }
            else
            {
                ChooseRandomDirection();
            }
        }
        else
        {
            //Launch projectile
            Projectile pro = Instantiate(projectile.gameObject).GetComponent<Projectile>();
            pro.transform.position = transform.position + forward * 1.5f;
            pro.Launch(forward);
            ChooseRandomDirection();
            state = EnemyState.Move;
        }
    }
}
