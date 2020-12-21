using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textbox : MonoBehaviour
{
    Text myText;
    [SerializeField]
    bool hideAtStart = false;
    private void Awake()
    {
        myText = GetComponentInChildren<Text>();
        gameObject.SetActive(!hideAtStart);
    }
    public void ShowText(string text)
    {
        gameObject.SetActive(true);
        myText.text = text;
    }
}
