using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [Header("Points life")]
    public float LifeMax = 150, Life = 150;
    [SerializeField] Image bloodScreen;

    private void Start()
    {
        Life = LifeMax;
    }

    private void Update()
    {
        UpdateUILife();
    }

    void UpdateUILife()
    {
        bloodScreen.color = new Color(255,255,255, (1/Life / LifeMax) * 255);
    }

    public void TakeDamage(int _dmg)
    {
        Life -= _dmg;

        if (Life <= 0)
        {
            Debug.Log("MORT ");
        }
    }
}
