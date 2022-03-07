using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBoitier : MonoBehaviour
{
    public Perks MyPerk;
    public int Cost = 2500;

    private void OnGUI()
    {
        transform.name = MyPerk.ToString() + " Botier";
    }

}
