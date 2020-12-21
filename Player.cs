using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    float moveSpeed = 5;
    Vector3 forward = Vector3.zero;
    Vector3 facing = Vector3.up;

    List<MagicItem> inventory;
    public Vector3 Facing
    {
        get
        {
            return facing;
        }
    }
    DamageDealer dealer;
    DamageTaker taker;

    Sword mySword;
    int gold = 0;

    MagicItem current;
    bool isPress = false;
    bool isFrozen = false;

    public IntEvent OnCoinChange
    {
        get;
        private set;
    }

    int keys = 0;

    public int Keys
    {
        get => keys;
        set
        {
            keys = value;
            OnKeyChange.Invoke(keys);
        }
    }
    public IntEvent OnKeyChange
    {
        get;
        private set;
    }

    public EquipmentEvent OnEquipmentChange
    {
        get;
        private set;
    }
    InteractiveObject carriedObject;
    public bool IsCarrying
    {
        get
        {
            return carriedObject != null;
        }
    }

    void DropObject()
    {
        carriedObject.transform.SetParent(transform.parent);
        carriedObject.transform.position = transform.position + facing * 1.25f;
        carriedObject.GetComponent<Projectile>()?.Launch(facing);
    }

    public void CarryObject(InteractiveObject obj)
    {
        carriedObject = obj;
        carriedObject.transform.SetParent(transform);
        carriedObject.transform.localPosition = new Vector3(0, 0.25f, -1);
    }
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        dealer = GetComponent<DamageDealer>();
        taker = GetComponent<DamageTaker>();

        mySword = GetComponentInChildren<Sword>();
        mySword.gameObject.SetActive(false);

        OnCoinChange = new IntEvent();
        OnKeyChange = new IntEvent();
        OnEquipmentChange = new EquipmentEvent();
        taker.OnHealthChanged.AddListener((int hp) =>
        {
            HealthBar.Instance.UpdateHearts(hp);
            
            if (hp > 0)
            {
                taker.BecomeInvincible(0.25f);
            }
        });

        

    }

    private void Start()
    {
        taker.OnDeath.RemoveListener(taker.Die);

    }

    public void Init()
    {
        inventory = new List<MagicItem>();
        GameManager.Instance.OnStateChanged.AddListener(() =>
        {
            if (GameManager.Instance.IsInGame)
            {
                Unfreeze();
            } else
            {
                Freeze();
            }
        });
    }
    public UnityEvent OnDeath
    {
        get
        {
            return taker.OnDeath;
        }
    }

    public void Heal(int health)
    {
        taker.Heal(health);
    }
    public void PickupItem(Pickup pickup)
    {
        switch(pickup.Type)
        {
            case PickupType.HealthRefill:
                Heal(4);
                AudioManager.Instance.PlayClip("Pickup", "Heart");
                break;
            case PickupType.Money:
                gold++;
                OnCoinChange.Invoke(gold);
                AudioManager.Instance.PlayClip("Pickup", "Coin");
                break;
            case PickupType.HealthIncrease:
                taker.Heal(taker.maxHP);
                taker.maxHP += 4;
                AudioManager.Instance.PlayClip("Pickup", "Heart Container");
                UIManager.Instance.ShowMessage("Your heart total has increased by 1");
                UIManager.Instance.OnMessageHidden.AddListener(GameManager.Instance.WinGame);
                HealthBar.Instance.ModifyMaxHealth(taker.maxHP);
                break;
            case PickupType.Item:
                //Add to inventory;
                break;
            case PickupType.Key:
                Keys++;
                AudioManager.Instance.PlayClip("Pickup", "Key");
                break;
            case PickupType.Arrow:
                if (HasItem(InventoryItems.Bows)) {
                    (GetItemOfType(InventoryItems.Bows) as Bow).Ammo += 5;
                }
                break;
            case PickupType.Bomb:
                if (HasItem(InventoryItems.Bombs))
                {
                    (GetItemOfType(InventoryItems.Bombs) as Bombs).Ammo++;
                } else
                {
                    GainItem(new Bombs());
                }
                break;
            case PickupType.GoddessCoin:
                AudioManager.Instance.PlayClip("Pickup", "Goddess Coin");
                Timer waitTimer = new Timer(0.25f);
                waitTimer.OnComplete.AddListener(() =>
                {
                    CameraManager.Instance.CurrentRoom.SpawnMonsters();
                });
                waitTimer.Start();
                break;
        }
    }

    public int Coins
    {
        get
        {
            return gold;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            transform.position += forward * moveSpeed * Time.deltaTime;
        }
    }

    public void OnMove(InputValue value)
    {
        if (!isFrozen)
        {
            forward = value.Get<Vector2>();
            bool isIdling = forward.sqrMagnitude < Mathf.Epsilon;
            myAnimator.SetBool("IsIdle", isIdling);

            if (!isIdling)
            {
                Directions closest = DirectionHandler.GetClosestDirection(forward);
                myAnimator.SetInteger("Direction", (int)closest);
                facing = DirectionHandler.ConvertDirectionToVector3(closest);
                forward = facing;
            }
        } 
        
    }

    public void Freeze()
    {
        isFrozen = true;
        myAnimator.SetBool("IsIdle", true);
        forward = Vector2.zero;
    }

    public void Unfreeze()
    {
        isFrozen = false;
    }

    public void OnInteract()
    {
        if (GameManager.Instance.IsInGame)
        {
            if (IsCarrying)
            {
                DropObject();
            }
            else
            {
                InteractiveObject.Current?.Interact();
            }
        }
    }

    public void OnSwingSword()
    {
        if (GameManager.Instance.IsInGame)
        {
            mySword.transform.localPosition = facing;
            mySword.Swing();
        }
    }

    public void SetSecondaryItem(MagicItem item)
    {
        current = item;
        OnEquipmentChange.Invoke(false, current?.UISprite);
    }

    public void SetSecondaryItem(InventoryItems type)
    {
        current = GetItemOfType(type);
        OnEquipmentChange.Invoke(false, current?.UISprite);
    }
    public void OnSecondaryItem()
    {
        if (GameManager.Instance.IsInGame)
        {
            isPress = !isPress;
            if (current != null)
            {
                if (!current.IsActive && isPress)
                {
                    current.Activate();
                }
                else if (!isPress)
                {
                    current.Deactivate();
                }
            }
        }
    }

    public bool HasItem(InventoryItems item)
    {
        foreach(MagicItem mi in inventory)
        {
            if (mi.Type == item)
            {
                return true;
            }
        }
        return false;
    }

    public MagicItem GetItemOfType(InventoryItems item)
    {
        foreach (MagicItem mi in inventory)
        {
            if (mi.Type == item)
            {
                return mi;
            }
        }
        return null;
    }

    public void GainItem(MagicItem item)
    {
        if (!HasItem(item.Type))
        {
            UIManager.Instance.ShowMessage(item.AcquisitionMessage);
            inventory.Add(item);
        }
    }

    public void GainItemOfType(InventoryItems type)
    {
       switch(type)
        {
            case InventoryItems.Bombs:
                GainItem(new Bombs());
                break;
            case InventoryItems.Boots:
                //Don't have boots yet
                break;
            case InventoryItems.Bows:
                GainItem(new Bow());
                break;
            case InventoryItems.Ring:
                GainItem(new WindRing());
                break;
            case InventoryItems.Shield:
                GainItem(new MirrorShield());
                break;
            case InventoryItems.Staff:
                GainItem(new FlameStaff());
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            CameraManager.Instance.CurrentRoom = collision.GetComponentInParent<Door>().room; 
        }
    }

    public List<MagicItem> Inventory
    {
        get
        {
            return inventory;
        }
    }

    public void OnOpenInventory()
    {
        UIManager.Instance.ToggleInventoryMenu();
    }

    public void OnCancel()
    {
        UIManager.Instance.ToggleInventoryMenu();
    }
}

public class IntEvent:UnityEvent<int>
{

}

public class EquipmentEvent:UnityEvent<bool, Sprite> { }

public enum InventoryItems
{
    None,
    Shield,
    Boots,
    Bombs,
    Bows,
    Staff,
    Ring
}