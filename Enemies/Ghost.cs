using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    SpriteRenderer myRenderer;
    float fadeTime = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        StartFadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MoveToLocation(Vector3 pos)
    {
        transform.position = pos;
        StartFadeIn();
    }

    void StartFadeIn()
    {
        Timer fadeTimer = new Timer(1 / 60.0f, (int)(60 * fadeTime));
        fadeTimer.OnTick.AddListener(() =>
        {
            myRenderer.color = myRenderer.color.SetAlpha(fadeTimer.RunPercent);
        });
        fadeTimer.Start();
    }

    void StartFadeOut()
    {
        Timer fadeTimer = new Timer(1 / 60.0f, (int)(60 * fadeTime));
        fadeTimer.OnTick.AddListener(() =>
        {
            myRenderer.color = myRenderer.color.SetAlpha(1.0f - fadeTimer.RunPercent);
        });
        fadeTimer.Start();
    }
}
