using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance { get; private set; }
    [SerializeField]
    Image heart;

    List<Animator> hearts;
    Vector2 heartStart = new Vector2(-275, 0);
    Vector2 spacing = new Vector2(75, 0);
    private void Awake()
    {
        Instance = this;
        hearts = new List<Animator>();
        for(int i = 0; i < 3; i++)
        {
            AddHeart();
        }
    }

    public void UpdateHearts(int hp)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            Animator current = hearts[i];
            if (hp >= (i + 1) * 4)
            {
                current.SetTrigger("QuickFull");
                current.SetInteger("Quarters", 4);
            } else if (hp <= i *4)
            {
                current.SetTrigger("QuickEmpty");
                current.SetInteger("Quarters", 0);
            } else
            {
                current.SetInteger("Quarters", hp - (i * 4));
            }
        }
    }

    public void ModifyMaxHealth(int maxHP)
    {
        int totalHearts = maxHP / 4;
        if (totalHearts > hearts.Count)
        {
            for (int i = 0; i < totalHearts - hearts.Count; i++)
            {
                AddHeart();
            }
        }
    }

    void AddHeart()
    {
        Image newHeart = Instantiate(heart);
        newHeart.rectTransform.SetParent(transform);
        newHeart.rectTransform.localPosition = heartStart + spacing * hearts.Count;
        hearts.Add(newHeart.GetComponent<Animator>());
    }
}
