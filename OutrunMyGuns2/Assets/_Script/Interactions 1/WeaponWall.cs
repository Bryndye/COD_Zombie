using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWall : ItemInteraction
{
    public int WeaponCost = 500, AmmoCost = 200;
    public Weapon PrefabWeapon;
    [HideInInspector] public string Message;

    private void Awake()
    {
        Message = "Press E to buy " + PrefabWeapon.Name + " [" + WeaponCost + "]" + "\n" +
            " ammo for [" + AmmoCost + "]";       
    }
}
