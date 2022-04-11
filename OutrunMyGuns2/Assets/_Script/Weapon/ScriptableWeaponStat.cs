using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ScriptableWeaponStat : ScriptableObject
{
    [Header("Munitions")]
    public int AmmoMaxStock = 80;
    public int AmmoMaxMag = 8;
    public int AmmoStock = 36, AmmoMag = 8;
    public float ReloadTime = 1f;

    [Header("Fire")]
    public int Damage = 10;
    public int BulletsPerShoot = 1;
    public float FireRate = 0.4f;
    [Tooltip("Plus proche de 1 plus precis, plus proche de 0 moins precis")]
    public float Precision = 0.1f;
    public bool canHold = true;
    public AudioClip ClipShoot;

    [Header("Recoil")]
    public Vector3 recoil;
    public float snapiness, returnSpeed;

    [Header("Aim")]
    public int AimFov;
    public float SpeedToScoop;
}
