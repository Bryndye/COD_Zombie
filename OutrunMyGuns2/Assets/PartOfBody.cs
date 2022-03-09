using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBody : MonoBehaviour
{
    ZombieBehaviour zombieB;

    private void Awake()
    {
        zombieB = GetComponentInParent<ZombieBehaviour>();
    }

    public void TakeDamage(int _dmg, PlayerWeapon _player)
    {
        zombieB.TakeDamage(_dmg * 2, _player, true);
    }
}
