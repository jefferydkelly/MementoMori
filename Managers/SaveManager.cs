using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Databox;
public class SaveManager:MonoBehaviour
{
    [SerializeField]
    DataboxObject saveDatabase;

    public static SaveManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    public SaveSlotInfo GetSaveInfo(int slotNum)
    {
        SaveSlotInfo info = new SaveSlotInfo();
        info.name = "ERROR";
        info.deaths = -1;

        if (slotNum > 0 && slotNum < 4)
        {
            info.name = saveDatabase.GetData<StringType>(string.Format("Data 0{0}", slotNum), "Player Data", "Name").Value;
            info.deaths = saveDatabase.GetData<IntType>(string.Format("Data 0{0}", slotNum), "Player Data", "Deaths").Value;
        }

        return info;
    }

    public void Save(SaveSlot slot)
    {
        IntType deaths = saveDatabase.GetData<IntType>(string.Format("Data 0{0}", slot.SlotNumber), "Player Data", "Deaths");
        deaths.Value = slot.PlayerDeaths;
        saveDatabase.SaveDatabase();
    }
}

public struct SaveSlotInfo
{
    public string name;
    public int deaths;
}
