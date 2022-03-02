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

    [Header("Fire")]
    public bool CanShoot = false;
    public int BulletsPerShoot = 1;
    public float FireRate = 0.4f, TimeToShoot = 0;
    [Tooltip("Plus proche de 1 plus precis, plus proche de 0 moins precis")] 
    public float Precision = 0.8f;

    private void OnEnable()
    {
        CanShoot = false;
        TimeToShoot = 0;
    }

    private void Update()
    {
        if (!CanShoot)
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
            Anim.SetTrigger("Reload");
        }
        else
        {
            GetAmmo();
        }
    }

    private void GetAmmo()
    {
        MunitionsStock += MunitionChargeur;
        MunitionChargeur = MunitionsMaxChargeur;
        MunitionsStock -= MunitionsMaxStock;
    }

}
