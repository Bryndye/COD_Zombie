using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name = "M1911";

    public Animator Anim;

    [Header("Munitions")]
    public int MunitionsMaxStock = 80;
    public int MunitionsMaxChargeur = 8;
    public int MunitionsStock = 36, MunitionChargeur = 8;
    [HideInInspector] public bool IsReloading = false;
    
    [Header("Fire")]
    public int Damage = 10;
    public int BulletsPerShoot = 1;
    public float FireRate = 0.4f, TimeToShoot = 0;
    [HideInInspector] public bool CanShoot = false;
    public bool canHold = true;
    [Tooltip("Plus proche de 1 plus precis, plus proche de 0 moins precis")] 
    public float Precision = 0.8f;
    [HideInInspector] public AudioSource SFX;

    [Header("Recoil")]
    public Vector3 recoil;
    public float snapiness, returnSpeed;

    public Transform AimPos;

    private void Awake()
    {
        SFX = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        CanShoot = false;
        TimeToShoot = 0;
    }

    private void Update()
    {
        if (IsReloading)
        {
            CanShoot = false;
        }
        else if (!CanShoot && MunitionChargeur > 0)
        {
            TimeToShoot += Time.deltaTime;
            if (TimeToShoot >= FireRate)
            {
                CanShoot = true;
            }
        }
    }

    public void Reload()
    {
        if (Anim != null)
        {
            IsReloading = true;
            Anim.SetTrigger("Reload");
        }
        else
        {
            GetAmmo();
        }
    }

    private void GetAmmo()
    {
        int _needAmmo = MunitionsMaxChargeur - MunitionChargeur;
        if (_needAmmo >= MunitionsStock)
        {
            _needAmmo = MunitionsStock;
            MunitionsStock -= _needAmmo;
        }
        else
        {
            MunitionsStock -= _needAmmo;
        }
        MunitionChargeur += _needAmmo;

        IsReloading = false;
    }


    public void MaxAmmo()
    {
        MunitionsStock = MunitionsMaxStock;
        MunitionChargeur = MunitionsMaxChargeur;
    }

}
