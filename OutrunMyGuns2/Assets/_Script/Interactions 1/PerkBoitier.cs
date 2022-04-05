using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBoitier : MonoBehaviour
{
    public Perks MyPerk;
    public int Cost = 2500;
    [HideInInspector] public string Message;

    private void Awake()
    {
        Message = "Press E to buy " + MyPerk.ToString() + " for " + Cost;
    }
}
