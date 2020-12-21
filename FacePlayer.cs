using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    Animator myAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dif = GameManager.Instance.Player.transform.position - transform.position;
        myAnimator.SetInteger("Direction", (int)DirectionHandler.GetClosestDirection(dif.normalized));
    }
}
