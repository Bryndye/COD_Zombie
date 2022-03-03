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
        float _alpha = 1 - Life / LifeMax;

        if (_alpha > 255)  _alpha = 255;
        else if (_alpha < 0)  _alpha = 0;

        bloodScreen.color = new Color(1,1,1, _alpha);
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
