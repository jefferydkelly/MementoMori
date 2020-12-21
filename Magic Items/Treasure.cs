using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]
    InventoryItems type;

    public InventoryItems Type
    {
        get
        {
            return type;
        }
    }
}
