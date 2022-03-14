using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeKill { Head, cut, normal}
public class PartOfBody : MonoBehaviour
{
    ZombieBehaviour zombieB;

    private void Awake()
    {
        zombieB = GetComponentInParent<ZombieBehaviour>();
    }

    public void TakeDamage(int _dmg, PlayerWeapon _player, TypeKill _type = TypeKill.Head)
    {
        if (_type == TypeKill.Head)
        {
            zombieB.TakeDamage(_dmg * 2, _player, TypeKill.Head);
        }
        else if (_type == TypeKill.cut)
        {
            zombieB.TakeDamage(_dmg, _player, TypeKill.cut);
        }
    }
}
