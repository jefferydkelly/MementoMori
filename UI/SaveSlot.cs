using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField]
    Text nameText;

    [SerializeField]
    Text deathText;
    int deaths = 0;

    [SerializeField]
    int slotNumber = -1;

    Button myButton;
    // Start is called before the first frame update
    void Start()
    {
        SaveSlotInfo info = SaveManager.Instance.GetSaveInfo(slotNumber);
        if (info.name != "ERROR")
        {
            PlayerName = info.name;
            PlayerDeaths = info.deaths;
            myButton = GetComponent<Button>();
            myButton.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        GameManager.Instance.StartGame(this);
    }

    public string PlayerName
    {
        get
        {
            return nameText.text;
        }

        set
        {
            nameText.text = value;
        }
    }

    public int PlayerDeaths
    {
        get
        {
            return deaths;
        }

        set
        {
            deaths = value;
            if (deathText)
            {
                deathText.text = deaths.ToString("D3");
            }
        }
    }

    public int SlotNumber
    {
        get
        {
            return slotNumber;
        }
    }

    public void Save()
    {
        SaveManager.Instance.Save(this);
    }
}
