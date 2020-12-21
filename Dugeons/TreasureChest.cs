using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : InteractiveObject
{
    [HideInInspector]
    public Treasure contents;
    [SerializeField]
    AudioClip openFX;
    bool hasBeenOpened = false;

    Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        if (!hasBeenOpened)
        {
            myAnimator.SetTrigger("Open");
            hasBeenOpened = true;
            GameObject tesoro = Instantiate(contents).gameObject;
            tesoro.transform.position = transform.position + Vector3.up * 1.5f;
            GameManager.Instance.Player.GainItemOfType(contents.Type);
            AudioManager.Instance.PlaySound(openFX);
            UIManager.Instance.OnMessageHidden.AddListener(() =>
            {
                Destroy(tesoro);
            });
        }
    }

    
}
