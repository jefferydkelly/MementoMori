using Databox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager
{
    DataboxObjectManager databaseManager;
    public static DatabaseManager Instance
    {
        get;
        set;
    }

    public DatabaseManager(DataboxObjectManager dom)
    {
        databaseManager = dom;
        databaseManager.GetDataboxObject("Items").LoadDatabase();
        databaseManager.GetDataboxObject("Tilesets").LoadDatabase();
        databaseManager.GetDataboxObject("Sounds").LoadDatabase();
        databaseManager.GetDataboxObject("Monsters").LoadDatabase();

    }

    public GameObject GetGameObjectFromDataBase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<ResourceType>(tableID, entryID, valueID).Load() as GameObject;
    }

    public string GetStringFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<StringType>(tableID, entryID, valueID).Value;
    }

    public int GetIntFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<IntType>(tableID, entryID, valueID).Value;
    }

    public float GetFloatFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<FloatType>(tableID, entryID, valueID).Value;
    }

    public bool GetBoolFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<BoolType>(tableID, entryID, valueID).Value;
    }

    public AudioClip GetClipFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        return databaseManager.GetDataboxObject(dbName).GetData<ResourceType>(tableID, entryID, valueID).Load() as AudioClip;
    }

    public Sprite GetSpriteFromDatabase(string dbName, string tableID, string entryID, string valueID)
    {
        Texture2D tex = databaseManager.GetDataboxObject(dbName).GetData<ResourceType>(tableID, entryID, valueID).Load() as Texture2D;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public Databox.Dictionary.OrderedDictionary<string, Databox.DataboxObject.DatabaseEntry> GetEntriesFromTable(string dbName, string tableID)
    {
        return databaseManager.GetDataboxObject(dbName).GetEntriesFromTable(tableID);
    }

}
