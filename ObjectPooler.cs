using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectPooler
{
    Dictionary<string, int> startIndices;
    List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    public void Init()
    {
        pooledObjects = new List<GameObject>();
        startIndices = new Dictionary<string, int>();
        int ind = 0;

        foreach (ObjectPoolItem item in itemsToPool)
        {
            startIndices[item.objectToPool.name] = ind;
            ind += item.amountToPool;
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = GameObject.Instantiate(item.objectToPool);
                obj.name = item.objectToPool.name;
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (startIndices.ContainsKey(tag))
        {
            int startIndex = startIndices[tag];
            for (int i = startIndex; i < pooledObjects.Count; i++)
            {
                GameObject pooler = pooledObjects[i];
                if (pooler.name == tag && !pooler.activeInHierarchy)
                {
                    pooler.SetActive(true);
                    return pooler;
                }
            }
        }
        return null;
    }

    public void ResetPool()
    {
        foreach (GameObject go in pooledObjects)
        {
            go.SetActive(false);
        }
    }
}

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool;
}