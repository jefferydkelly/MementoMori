using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Textbox priceText;

    [SerializeField]
    Textbox coinText;

    [SerializeField]
    Textbox keyText;

    [SerializeField]
    InventoryMenu inventoryMenu;

    [SerializeField]
    ItemBox primaryBox;

    [SerializeField]
    ItemBox secondaryBox;

    [SerializeField]
    Textbox messageBox;

    Timer messageTimer;
    float messageRevealTime = 5.0f;
    static UIManager instance;
    public static UIManager Instance
    {
        get { 
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    public UnityEvent OnMessageHidden
    {
        get
        {
            return messageTimer.OnComplete;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        messageTimer = new Timer(messageRevealTime);
        messageTimer.OnComplete.AddListener(HideMessage);
    }
    // Start is called before the first frame update

    public void Initialize()
    {
        GameManager.Instance.Player.OnCoinChange.AddListener((int coin) => {
            coinText.ShowText(coin.ToString("D4"));
        });

        GameManager.Instance.Player.OnKeyChange.AddListener((int keys) =>
        {
            keyText.ShowText(keys.ToString("D2"));
        });

        GameManager.Instance.Player.OnEquipmentChange.AddListener((bool isPrimary, Sprite sprite) =>
        {
            if (isPrimary)
            {
                primaryBox.SetSprite(sprite);
            }
            else
            {
                secondaryBox.SetSprite(sprite);
            }
        });
    }

    public void ShowItemInfo(ItemInfo info)
    {
        priceText.ShowText(string.Format("{0}\n{1}", info.name, info.basePrice));
    }

    public void HideItemInfo()
    {
        priceText.gameObject.SetActive(false);
    }

    public void ShowMessage(string msg, bool startTimer = true)
    {
        GameManager.Instance.State = GameState.ShowingMessage;
        messageBox.gameObject.SetActive(true);
        messageBox.ShowText(msg);
        if (startTimer)
        {
            messageTimer.Start();
        }
    }

    public void HideMessage()
    {
        messageBox.gameObject.SetActive(false);
        GameManager.Instance.State = GameState.InGame;
        OnMessageHidden.RemoveAllListeners();
        OnMessageHidden.AddListener(HideMessage);
    }
    public void ToggleInventoryMenu()
    {
        
        
        if (inventoryMenu.isActiveAndEnabled)
        {
            inventoryMenu.gameObject.SetActive(false);
            inventoryMenu.enabled = false;
            GameManager.Instance.State = GameState.InGame;
            HideMessage();
        } else
        {
            inventoryMenu.gameObject.SetActive(true);
            inventoryMenu.enabled = true;
            GameManager.Instance.State = GameState.ShowingMessage;
            
        }
        
    }
}
