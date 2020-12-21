using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : Enemy
{
    [SerializeField]
    GameObject elementTrail;

    [SerializeField]
    Projectile projectile;

    float trailDropTime = 1.0f;
    Timer trailDropTimer;

    Timer stateSwitchTimer;
    float stateSwitchWait = 3.0f;

    Timer fireTimer;
    float fireTime = 1.0f;

    ElementalState state = ElementalState.Static;
    // Start is called before the first frame update
    void Awake()
    {
        trailDropTimer = new Timer(trailDropTime, true);
        trailDropTimer.OnTick.AddListener(() =>
        {
            GameObject trail = Instantiate(elementTrail);
            trail.transform.SetParent(transform.parent);
            trail.transform.position = transform.position - forward;
        });

        fireTimer = new Timer(fireTime, true);
        fireTimer.OnTick.AddListener(Fire);
        stateSwitchTimer = new Timer(stateSwitchWait, true);
        stateSwitchTimer.OnTick.AddListener(SwitchState);
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("IsIdle", false);
        
    }

    private void OnEnable()
    {
        stateSwitchTimer.Start();
    }

    private void OnDisable()
    {
        stateSwitchTimer.Stop();
        fireTimer.Stop();
        trailDropTimer.Stop();
    }

    private void OnDestroy()
    {
        stateSwitchTimer.Stop();
        fireTimer.Stop();
        trailDropTimer.Stop();
    }


    void SwitchState()
    {
        trailDropTimer.Stop();
        fireTimer.Stop();
        if (Random.value < 0.5f)
        {
            FacePlayer();
            Dash();
        } else
        {
            FacePlayer();
            state = ElementalState.Fire;
            fireTimer.Start();
        }
        /*
        int nextState = Random.Range(0, 2);
        switch(state)
        {
            case ElementalState.Static:
                if (nextState == 0)
                {
                    state = ElementalState.Dash;
                } else
                {
                    state = ElementalState.Fire;
                }
                break;
            case ElementalState.Dash:
                if (nextState == 0)
                {
                    state = ElementalState.Static;
                }
                else
                {
                    state = ElementalState.Fire;
                }
                break;
            case ElementalState.Fire:
                if (nextState == 0)
                {
                    
                }
                else
                {
                    state = ElementalState.Static;
                }
                break;
        }
        */
    }

    void FacePlayer()
    {
        Vector2 dif = GameManager.Instance.Player.transform.position - transform.position;
        forward = dif.normalized;
        myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(dif));
    }

    void Dash()
    {
        state = ElementalState.Dash;
        trailDropTimer.Start();
        forward = Vector2.one.Rotate(Random.Range(0, 8) * 45);
        myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(forward));
    }

    void Fire()
    {
        Projectile proj = Instantiate(projectile);
        proj.transform.SetParent(transform.parent);
        proj.transform.position = transform.position + forward * 1.5f;
        proj.Launch(forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 fwd = -forward;
            fwd = fwd.Rotate(Random.Range(-2, 3) * 45);
            forward = fwd;
            myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(forward));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ElementalState.Dash)
        {
            Move(Time.deltaTime);
        }
    }
}

public enum ElementalState
{
    Static,
    Dash,
    Fire
}
