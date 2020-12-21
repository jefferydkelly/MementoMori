using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItem
{
    public InventoryItems Type
    {
        get;
        protected set;
    }

    public Sprite UISprite
    {
        get;
        protected set;
    }

    public bool IsActive
    {
        get;
        protected set;
    }

    public string Description
    {
        get;
        protected set;
    }

    public string ItemName
    {
        get;
        protected set;
    }

    public string AcquisitionMessage
    {
        get
        {
            return string.Format("You got the {0}.\n{1}", ItemName, Description);
        }
    }

    public MagicItem()
    {
        IsActive = false;
    }
    public virtual void Activate() 
    { 
        if (!IsActive)
            IsActive = true; 
    }

    public virtual void Deactivate() 
    { 
        IsActive = false; 
    }
}
