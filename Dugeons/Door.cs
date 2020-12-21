using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractiveObject
{
    public Room room;

    Animator myAnimator;

    DoorState state = DoorState.Open;


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    public bool IsOpen
    {
        get
        {
            return state == DoorState.Open;
        }
        set
        {
            if (!IsLocked)
            {
                state = value ? DoorState.Open : DoorState.Closed;
                myAnimator.SetBool("IsOpen", value);
            }
        }
    }

    public bool IsLocked
    {
        get
        {
            return state == DoorState.Locked;
        }
        set
        {
            state = value ? DoorState.Locked : DoorState.Open;
            myAnimator.SetBool("IsLocked", value);
        }
    }

    public override void Interact()
    {
        if (IsLocked && GameManager.Instance.Player.Keys > 0)
        {
            GameManager.Instance.Player.Keys--;
            IsLocked = false;
            IsOpen = true;
        }
    }

}

public enum DoorState
{
    Open,
    Closed,
    Locked
}
