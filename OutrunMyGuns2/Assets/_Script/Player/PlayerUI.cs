using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MessageBonus;
    public GameObject InstaKillUI, DoublePointsUI;

    private void Awake()
    {
        MessageBonus.gameObject.SetActive(false);
    }

    public void Effect(string _msg)
    {
        MessageBonus.text = _msg;
        MessageBonus.gameObject.SetActive(false);
        MessageBonus.gameObject.SetActive(true);
    }
}
