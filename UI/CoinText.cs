using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{
    public static CoinText Instance { get; private set; }
    Text myText;

    private void Awake()
    {
        Instance = this;
        myText = GetComponent<Text>();
    }

    public void SetText(int i)
    {
        myText.text = i.ToString("D4");
    }
}
