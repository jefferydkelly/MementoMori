using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Vector3 forward;
    
    [SerializeField]
    float moveSpeed = 2.0f;
    protected Animator myAnimator;
    [SerializeField]
    EnemyLevel toughness = EnemyLevel.Basic;

    protected EnemyState state;

    [SerializeField]
    float stateChangeTime = 2.0f;
    Timer stateChangeTimer;

    List<ItemInfo> loot;
    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        
        myAnimator = GetComponent<Animator>();
        myAnimator.SetBool("IsIdle", false);
        
        //myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(forward));

        loot = DungeonGenerator.Instance.GetLootForLevel(toughness);
        ChooseRandomDirection();
        GetComponent<DamageTaker>().OnDeath.AddListener(DropLoot);
        
        state = EnemyState.Move;
        stateChangeTimer = new Timer(stateChangeTime, true);
        stateChangeTimer.OnTick.AddListener(ChangeState);
        GetComponent<DamageTaker>().OnDeath.AddListener(stateChangeTimer.Stop);
        stateChangeTimer.Start();
    }
    
    protected void ChooseRandomDirection()
    {
        Forward = DirectionHandler.ConvertDirectionToVector3((Directions)(Random.Range(0, 4)));
    }

    protected virtual void ChangeState()
    {
        ChooseRandomDirection();
    }

    void DropLoot()
    {
        if (Random.value >= 0.5f)
        {
            float maxScatterDistance = 1;
            ItemInfo lootDrop = loot.RandomElement(); ;
            for (int i = 0; i < Random.Range(0, lootDrop.maxDropSize) + 1; i++)
            {
                GameObject coin = Instantiate(lootDrop.item);
                Vector3 displacement = (Random.value * Mathf.PI * 2).FromRadianToVector();
                coin.transform.SetParent(transform.parent);
                coin.transform.localPosition = transform.localPosition + displacement * Random.value * maxScatterDistance;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsInGame)
        {
            if (state == EnemyState.Move)
            {

                Move(Time.deltaTime);
            }
        }
    }

    protected void Move(float dt)
    {
        if (IsSomethingAheadOfMe())
        {
            RotateAway();
        }
        transform.position += forward * moveSpeed * dt;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }

    bool IsSomethingAheadOfMe()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + forward, forward, 1.0f);
        return hit.collider != null && !hit.collider.CompareTag("Player");
    }

    public void RotateAway()
    {
        Vector2 fwd = forward;
        fwd = fwd.RotateDeg(Random.Range(1, 4) * 90);
        Forward = fwd;
        stateChangeTimer.Restart();
    }

    protected Vector3 Forward
    {
        get
        {
            return forward;
        }

        set
        {
            forward = value;
            myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(forward));
        }
    }

    protected Timer StateChangeTimer
    {
        get
        {
            return stateChangeTimer;
        }
    }
}

public enum EnemyLevel
{
    Basic,
    Advanced,
    Miniboss,
    Boss
}

public enum EnemyState
{
    Idle,
    Move,
    Attack
}